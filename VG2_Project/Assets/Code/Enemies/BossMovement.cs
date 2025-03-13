using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossMovement : MonoBehaviour
{
    //Outlets
    public Rigidbody2D _rb;
    public CircleCollider2D _col;
    
    //State Tracking
    public float walkTimer;
    public float waitTimer;
    public Vector2 direction;
    public bool isActive;
    private int choiceDirection;

    public float walkSpeed;

    public float bearNumber;

    // Awake occurs before Start
    void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _col = this.GetComponent<CircleCollider2D>();

        walkTimer = 0f;
        waitTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        { 
            walkTimer += Time.deltaTime;
        }
    }

    void ChangeDirection()
    {
        //Determine which direction the character will move.
        choiceDirection = Random.Range(0, 4);
        if (choiceDirection == 0)
        {
            direction = Vector2.up;
        }
        if (choiceDirection == 1)
        {
            direction = Vector2.down;
        }
        if (choiceDirection == 2)
        {
            direction = Vector2.left;
        }
        if (choiceDirection == 3)
        {
            direction = Vector2.right;
        }
    }

    void MoveInDirection()
    {
        _rb.AddForce(direction * (walkSpeed * Time.deltaTime), ForceMode2D.Impulse);
    }
}
