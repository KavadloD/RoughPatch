using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoomerPickup : MonoBehaviour
{
    //outlets
    public GameObject player;
    public CircleCollider2D _col;
    public GameObject _notices;
    
    // Start is called before the first frame update
    void Start()
    {
        _col = this.GetComponent<CircleCollider2D>();
        _notices.SetActive(false);
    }

    public void GetBoomer()
    {
        _notices.SetActive(true);
        player.GetComponent<PlayerController>().receivedBoomerang = true;
        Destroy(gameObject);
    }
}
