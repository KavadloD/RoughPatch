using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    //Outlets
    public GameObject PointA;
    public GameObject PointB;
    public GameObject PointC;

    public GameObject bossProjectile;
    public GameObject enemyPrefab;
    public GameObject player;
    
    //State Tracking
    
    //Movement
    public float startingMoveTime;
    public float moveSpeed;
    public bool stopped;
    public GameObject currentPoint;
    public float bossWaitTime;
    
    private float moveTime;
    private float distFromA;
    private float distFromC;

    public bool movetoA;
    public bool movetoC;
    
    //Projectile
    public float projectileTime;
    private  float tempProjectileTime;

    //Health
    public float health;
    public float maxHealth;
    
    //Color
    public float flashTime;
    private Color origionalColor;
    private Renderer rend;

    float r ;  // red component
    float g ;  // green component
    float b ;  // blue component

    
    
    // Start is called before the first frame update
    void Start()
    {
        PointA = GameObject.Find("Point A");
        PointB = GameObject.Find("Point B");
        PointC = GameObject.Find("Point C");

        player = GameObject.Find("Player");
        
        moveTime = startingMoveTime;
        stopped = false;

        movetoA = true;
        movetoC = false;

        health = maxHealth;
        
        rend = GetComponent<Renderer> ();
        origionalColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime -= Time.deltaTime;

        tempProjectileTime -= Time.deltaTime;
        
        //print(PointA.transform);
        //print(PointC.transform);
        
        if (tempProjectileTime <= 0 && !stopped)
        {
            FireProjectile();

            tempProjectileTime = projectileTime;
        }

        if (moveTime <= 0 && !stopped)
        {
            stopped = true;
            StartCoroutine(BossAttack());
        }

        if (!stopped)
        {
            MoveBoss();
        }

        if (Vector2.Distance(transform.position, PointA.transform.position) == 0)
        {
            movetoA = false;
            movetoC = true;
        }
        
        if (Vector2.Distance(transform.position, PointC.transform.position) == 0)
        {
            movetoA = true;
            movetoC = false;
        }

        if (health <= 0)
        {
            player.GetComponent<PlayerController>().bossDead = true;
            //Destroy(gameObject);
        }
    }

    //Moves boss back and forth between two points
    void MoveBoss()
    {
        if (movetoA)
        {
            transform.position = 
                Vector2.MoveTowards(transform.position, PointA.transform.position, moveSpeed * Time.deltaTime);
        }

        if (movetoC)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, PointC.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    
    //Creates projectile
    void FireProjectile()
    {
        Instantiate(bossProjectile, transform.position, Quaternion.identity);
    }

    //Creates enemies
    public void SpawnEnemies()
    {
        int randomInt = Random.Range(0, 2);
        
        if (randomInt == 0)
        {
            Instantiate(enemyPrefab, new Vector3(10.5f, 8.5f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(enemyPrefab, new Vector3(25.5f, 8.5f, 0), Quaternion.identity);
        }
    }
    
    IEnumerator BossAttack()
    {
        SpawnEnemies();
        
        yield  return new WaitForSeconds(bossWaitTime);
        
        stopped = false;
        
        moveTime = startingMoveTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 10)
        {
            health -= 1;
            FlashRed();
        }
    }
    
    //Flashes boss red
    void FlashRed()
    {
        GetComponent<Renderer>().material.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    //Returns boss to original color
    void ResetColor()
    {
        GetComponent<Renderer>().material.color = origionalColor;
    }
}
