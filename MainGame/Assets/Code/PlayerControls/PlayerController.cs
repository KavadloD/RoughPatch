using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    //Outlets (Data from other GameObjects/Components)
   
    //Player
    public Transform _trans;
    public Rigidbody2D _rb;
    public CapsuleCollider2D _col;
    public SpriteRenderer _spr;
    public Animator _anim;
    public TrailRenderer _trail;
    
    //Scythe
    public CapsuleCollider2D _scytheCol;
    public SpriteRenderer _scytheSpr;
    public Transform scythe;

    //Boomerang
    public Rigidbody2D _boomerRb;

    //Enemy
    private Vector2 enemyVelocity;
    private Rigidbody2D _enemyRb;

    //Generated Prefabs
    public GameObject boomerang;
    public CameraController _CameraController;
    
    //Audio
    public AudioManager audioController;

    //State Tracking (Variables unique to this GameObject/Component)
    
    //Player
    public float panDuration;
    public float _moveSpeed;
    public bool isActive;
    private Vector2 activeVelocity;
    public Vector2 storedPlayerDirection;

    //Dodge
    public bool _canDodge;
    public float _dodgeDistance;
    public float _dodgeCooldown;
    
    //Attack
    public bool _canAttack;
    public float _attackBuildUp;
    public float _atackCooldown;
    
    //Health
    public float health;
    public float maxHealth;
    public float invincibleTime;
    public float knockBack;
    public bool canTakeDamage;
    
    //Boomerang
    public bool canThrowBoom;
    public bool returnBoomerang;
    public float boomerSpeed;
    public float boomerThrowTime;
    public float boomerWaitTime;
    public int holdCounter;

    public bool receivedBoomerang;
    
    //Scene
    public bool bossDead;

    // Awake is called before Start. I like to use it to assign Outlets and such.
    void Awake()
    {
        _trans = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();
        _trail = GetComponent<TrailRenderer>();

        _trail.enabled = false;
        
        storedPlayerDirection = Vector2.down;
        
        health = 6;
        maxHealth = 6;
        
        //scythe = this.transform.GetChild(0);
        _scytheCol = scythe.GetComponent<CapsuleCollider2D>();
        _scytheSpr = scythe.GetComponent<SpriteRenderer>();

        _scytheCol.enabled = false;
        _scytheSpr.enabled = false;

        _boomerRb = boomerang.GetComponent<Rigidbody2D>();
        boomerang.SetActive(false);

        isActive = true;
        _canDodge = true;
        _canAttack = true;
        canTakeDamage = true;

        canThrowBoom = true;
        returnBoomerang = false;
        receivedBoomerang = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossDead)
        {
            SceneManager.LoadScene("WinScreen");
        }
        
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOverScreen");
        }
        
        //No more than 6 HP
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        
        if (returnBoomerang)
        {
            StopCoroutine(ThrowBoomerang());
            
            boomerang.gameObject.transform.position = Vector2.MoveTowards(boomerang.transform.position, transform.position, boomerSpeed  * Time.deltaTime);

            if (boomerang.gameObject.transform.position == transform.position)
            {
                disableBoomerang();
            }
        }

        if (isActive == true && _canDodge && _canAttack)
        {
            //zuzupets
            float movementSpeed = _rb.velocity.sqrMagnitude;
            _anim.SetFloat("Speed", movementSpeed);
            if (movementSpeed > 0.1f)
            {
                _anim.SetFloat("moveUp", _rb.velocity.x);
                _anim.SetFloat("moveDown", _rb.velocity.y);
            }

            //Player Movement
            if (Input.GetKey(KeyCode.W))
            {
                _rb.AddForce(Vector2.up * (_moveSpeed * Time.deltaTime), ForceMode2D.Impulse);

                storedPlayerDirection = Vector2.up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rb.AddForce(Vector2.left * (_moveSpeed * Time.deltaTime), ForceMode2D.Impulse);

                storedPlayerDirection = Vector2.left;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rb.AddForce(Vector2.right * (_moveSpeed * Time.deltaTime), ForceMode2D.Impulse);

                storedPlayerDirection = Vector2.right;
            }

            if (Input.GetKey(KeyCode.S))
            {
                _rb.AddForce(Vector2.down * (_moveSpeed * Time.deltaTime), ForceMode2D.Impulse);

                storedPlayerDirection = Vector2.down;
            }

            //Reset shortcut
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            //Dodge roll
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dodge());
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.M))
            {
                StartCoroutine(Attack());
                _anim.SetTrigger("attack");
            }

            //Boomerang
            if (canThrowBoom && receivedBoomerang && _canDodge)
            {
                if (Input.GetKey(KeyCode.L))
                {
                    holdCounter += 1;
                }

                if (Input.GetKeyUp(KeyCode.L))
                {
                    StartCoroutine(ThrowBoomerang());
                }
            }
        }
    }

    //Activates on collision
    void OnTriggerEnter2D(Collider2D col)
    {
        //Check if we collided with an enemy
        if (col.gameObject.layer == 3 || col.gameObject.layer == 12)
        {
            Vector3 knockBackDirection = transform.position - col.gameObject.transform.position;

            StartCoroutine(TakeDamage(knockBackDirection));
        }

        if (col.GetComponent<BoomerPickup>())
        {
            col.GetComponent<BoomerPickup>().GetBoomer();
        }
        
        //Checks if we collided with room
        if (col.gameObject.layer == 8)
        {
            //room collider child object set to false
            col.transform.Find("Camera Colliders").gameObject.SetActive(false);
            
            col.GetComponent<SpawnEnemies>().SpawnEnemy();

            if (col.gameObject.name == "Boss Room Collider")
            {
                audioController.inBossRoom = true;
            }
        }
        
        //Checks if we collided with camera colliders
        if (col.gameObject.layer == 6 && col.name == "Up Camera Collider")
        {
            ShiftPlayer(col.name);
            _CameraController.PanUp();
        }
        else if(col.gameObject.layer == 6 && col.name == "Down Camera Collider")
        {
            ShiftPlayer(col.name);
            _CameraController.PanDown();
        }
        else if(col.gameObject.layer == 6 && col.name == "Left Camera Collider")
        {
            ShiftPlayer(col.name);
            _CameraController.PanLeft();
        }
        else if(col.gameObject.layer == 6 && col.name == "Right Camera Collider")
        {
            ShiftPlayer(col.name);
            _CameraController.PanRight();
        }
    }

    //Activates upon leaving a room
    private void OnTriggerExit2D(Collider2D col)
    {
        //Check if we left a room
        if (col.gameObject.layer == 8)
        {
            StartCoroutine(WaitToRemoveColliders(col));
            //col.GetComponent<RoomController>().DisableEnemies();
        }
    }

    //Temporarily sets collider to active in front of player with a cooldown
    IEnumerator Attack()
    {
        _canAttack = false;
        _anim.SetTrigger("attack");

        yield return new WaitForSeconds(_attackBuildUp);

        // Define attack area
        Vector2 attackPosition = (Vector2)transform.position + storedPlayerDirection * 1.2f;
        Vector2 attackSize = new Vector2(1.5f, 1f); // Adjust size as needed

        // Perform BoxCast in the attack direction
        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackPosition, attackSize, 0f, Vector2.zero);

        // Debug: Draw the actual BoxCast hitbox
        //DrawBoxCast(attackPosition, attackSize);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer == 3) // Enemy layer
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.health -= 1;
                }
            }
        }

        yield return new WaitForSeconds(_atackCooldown);
        _canAttack = true;
    }

    void DrawBoxCast(Vector2 position, Vector2 size)
    {
        Vector2 topLeft = position + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = position + new Vector2(size.x / 2, size.y / 2);
        Vector2 bottomLeft = position + new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = position + new Vector2(size.x / 2, -size.y / 2);

        Debug.DrawLine(topLeft, topRight, Color.red, 0.2f);
        Debug.DrawLine(topRight, bottomRight, Color.red, 0.2f);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red, 0.2f);
        Debug.DrawLine(bottomLeft, topLeft, Color.red, 0.2f);
    }

    //Coroutune to dodge. Reused the code for movement and made it one burst with a cooldown.
    IEnumerator ThrowBoomerang()
    {
        boomerang.SetActive(true);
     
        Vector2 boomerVelocity = _rb.velocity;

        if (boomerVelocity == Vector2.zero)
        {
            boomerVelocity = storedPlayerDirection;
        }
        
        boomerang.transform.position = transform.position;
        _boomerRb.AddForce(boomerVelocity.normalized * boomerSpeed, ForceMode2D.Impulse);
        
        canThrowBoom = false;
        
        yield return new WaitForSeconds(boomerThrowTime);
            
        _boomerRb.AddForce((boomerVelocity.normalized * -boomerSpeed), ForceMode2D.Impulse);
        
        if (holdCounter > 120)
        {
            yield return new WaitForSeconds(boomerWaitTime);
        }

        returnBoomerang = true;
    }

    private void disableBoomerang()
    {
        returnBoomerang = false;
        canThrowBoom = true;
        boomerang.SetActive(false);
        holdCounter = 0;
    }
IEnumerator Dodge()
{
    if (!_canDodge) yield break; // Prevent multiple dodges at once

    _canDodge = false;
    isActive = false; // Temporarily disable movement input

    // Determine the dodge direction: use velocity if moving, otherwise use the last stored direction
    Vector2 dodgeDirection = _rb.velocity.magnitude > 0 ? _rb.velocity.normalized : storedPlayerDirection;

    _trail.enabled = true;
    Physics2D.IgnoreLayerCollision(6, 3, true); // Ignore collision with enemies

    _rb.velocity = dodgeDirection * _dodgeDistance; // Apply a burst of speed
    yield return new WaitForSeconds(0.2f); // Dodge duration

    _rb.velocity = Vector2.zero; // Stop the dodge movement
    Physics2D.IgnoreLayerCollision(6, 3, false); // Re-enable collision with enemies

    _trail.enabled = false;
    isActive = true; // Re-enable movement input

    yield return new WaitForSeconds(_dodgeCooldown); // Wait for cooldown before dodging again
    _canDodge = true;
}


    //Coroutine to activate camera colliders and then deactivate them after pan to next room
    IEnumerator WaitToRemoveColliders(Collider2D obj)
    {
        obj.transform.Find("Camera Colliders").gameObject.SetActive(true);
        yield return new WaitForSeconds(panDuration);
        obj.transform.Find("Camera Colliders").gameObject.SetActive(false);
    }

    //Moves the player into the next room when they hit a camera collider
    public void ShiftPlayer(string name)
    {
        if (name == "Up Camera Collider")
        {
            this.transform.position += (new Vector3(0, 1, 0));
        }
        else if (name == "Down Camera Collider")
        {
            this.transform.position += (new Vector3(0, -1, 0));
        }
        else if (name == "Right Camera Collider")
        {
            this.transform.position += (new Vector3(1, 0, 0));
        }
        else if (name == "Left Camera Collider")
        {
            this.transform.position += (new Vector3(-1, 0, 0));
        }
    }

    public void Flicker()
    {
        canTakeDamage = false;
        float flickerTime = 0f;
        flickerTime += Time.deltaTime;
        if (flickerTime <= 0.24f)
        {
            _spr.enabled = false;
        }

        if (flickerTime > 0.25f && flickerTime < 0.49f)
        {
            _spr.enabled = true;
        }
        
        if (flickerTime > 0.5f && flickerTime < 0.74f)
        {
            _spr.enabled = false;
        }
        
        if (flickerTime > 0.75f && flickerTime < 0.99f)
        {
            _spr.enabled = true;
        }
        
        if (flickerTime > 1f && flickerTime < 1.24f)
        {
            _spr.enabled = false;
        }
        
        if (flickerTime > 1.25f)
        {
            _spr.enabled = true;
        }
    }

    IEnumerator TakeDamage(Vector3 knockBackDirection)
    {
        //Push player away
        _rb.AddForce(knockBackDirection.normalized * knockBack, ForceMode2D.Impulse);
        
        //Lose one health if you can take damage
        if (canTakeDamage)
        {
            health -= 1;

            canTakeDamage = false;
        }

        yield return new WaitForSeconds(invincibleTime);

        canTakeDamage = true;
    }
    
    
}
