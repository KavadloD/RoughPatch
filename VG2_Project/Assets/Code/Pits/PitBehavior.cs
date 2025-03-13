using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitBehavior : MonoBehaviour
{
    //This component is placed on the Pit Objects to control when they are enabled/disabled

    //Outlets
    public GameObject player;
    public BoxCollider2D _col;
    
    // Start is called before the first frame update
    void Awake()
    {
        _col = this.GetComponent<BoxCollider2D>();
        _col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>()._canDodge == false)
        {
            _col.enabled = false;
        }
        else
        {
            _col.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().health -= 1f;
        }
        else if (other.GetComponent<EnemyController>())
        {
            other.GetComponent<EnemyController>().health -= 1f;
        }
    }
}
