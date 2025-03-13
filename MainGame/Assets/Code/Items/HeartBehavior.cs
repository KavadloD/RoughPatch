using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    //Outlets
    public Rigidbody2D _rb;
    public Collider2D _col;
    
    //State Tracking
    public GameObject _toucher;
    public float _pHP;
    public float _pMax;
    
    //On Collision
    public void OnCollisionEnter2D(Collision2D other)
    {
        _toucher = other.gameObject;
        if (_toucher.GetComponent<PlayerController>())
        {
            _pHP = _toucher.GetComponent<PlayerController>().health;
            _pMax = _toucher.GetComponent<PlayerController>().maxHealth;
            if (_pHP < _pMax)
            {
                _toucher.GetComponent<PlayerController>().health += 2;
                Destroy(gameObject);
            }
        }
    }
}
