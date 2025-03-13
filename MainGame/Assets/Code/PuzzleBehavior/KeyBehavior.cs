using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    //Outlets
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public CapsuleCollider2D _col;
    
    // Start is called before the first frame update
    void Awake()
    {
        _col = this.GetComponent<CapsuleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            door1.SetActive(false);
            door2.SetActive(false);
            door3.SetActive(false);
            Destroy(gameObject);
        }
    }
}
