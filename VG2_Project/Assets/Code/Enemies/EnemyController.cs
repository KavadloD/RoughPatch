using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour        
{
    // Outlets
    private Transform target;
    public GameObject tar;
    public GameObject heart;

    // State Tracking
    public float speed;
    public float stoppingDistance;
    public bool inRange;

    public bool isTouchingWall;

    public float aggroRange;

    public float health;

    public float lastDirectionChangeTIme;
    public float directionChangeTime;
    public float enemyVelocity;
    
    private Vector2 movementDirection;
    public Vector2 movementPerSecond;

    private bool spawnedHeart;

    // Animator
    Animator _BBanim;
    Rigidbody2D _BBrb;
    

    // Start is called before the first frame update
    void Awake()
    {
        tar = GameObject.Find("Player");
        target = tar.GetComponent<Transform>();
        
        lastDirectionChangeTIme = 0f;
        CalculateNewMovementDirection();

        _BBanim = GetComponent<Animator>();
        _BBrb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            speed = 0;
            spawnHeart();
            _BBanim.Play("Base Layer.BBdeath");
            
            Destroy(gameObject, 1.25f);
            return;
        }
        else
        {

            if (Vector2.Distance(transform.position, target.position) < aggroRange)
            {
                inRange = true;
            }
            else
            {
                inRange = false;
                _BBanim.SetTrigger("targetNotInRange");
            }

            //Checks if player is in range. Moves towards player if in range, moves Randomly otherwise
            if (inRange)
            {
                speed = 2.5f;

                //play attack anim
                _BBanim.Play("Base Layer.BBattack");

                if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
            }
            else if (isTouchingWall)
            {
                MoveAwayFromWall();
            }
            else
            {
                if (Time.time - lastDirectionChangeTIme > directionChangeTime)
                {
                    lastDirectionChangeTIme = Time.time;
                    CalculateNewMovementDirection();
                }

                MoveEnemy();
            }
        }

        //animator
        float BBmovementSpeed = _BBrb.velocity.sqrMagnitude;
        _BBanim.SetFloat("speed", BBmovementSpeed);
        if (BBmovementSpeed > 0.0001f)
        {
            _BBanim.SetFloat("movementX", _BBrb.velocity.x);
            _BBanim.SetFloat("movementY", _BBrb.velocity.y);
        }
    }

    public void KillBB()
    {
        spawnHeart();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Checks if enemy got hit
        if (col.gameObject.layer == 10)
        {
            health -= 1;
        }
        
        if (col.gameObject.layer == 9 || col.gameObject.layer == 11)
        {
            isTouchingWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
        
        if (other.gameObject.layer == 9 || other.gameObject.layer == 11)
        {
            isTouchingWall = false;
        }
    }


    //Chance to spawn heart upon killing enemy
    private void spawnHeart()
    {
        Vector3 enemyPos = gameObject.transform.position;
        
        if (!spawnedHeart)
        {
            int randomInt = Random.Range(0, 4);
            
            if (randomInt == 0)
            {
                Instantiate(heart, enemyPos, quaternion.identity);
            }
            
            spawnedHeart = true;
        }
    }

    //Picks random direction for enemy to move in
    private void CalculateNewMovementDirection()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * enemyVelocity;
    }

    private void MoveAwayFromWall()
    {
        movementPerSecond = movementDirection * -enemyVelocity;
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime));
    }

    //Moves the enemy forward
    private void MoveEnemy()
    {
        transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime));

    }
}
