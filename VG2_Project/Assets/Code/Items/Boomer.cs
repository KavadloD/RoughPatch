using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MonoBehaviour
{
    public float rotationSpeed;
    private Transform target;
    public GameObject tar;
    private float speed;

    void Awake()
    {
        speed = tar.GetComponent<PlayerController>().boomerSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,rotationSpeed) * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            //print("Enemy");
            other.transform.GetComponent<EnemyController>().health -= 1;
        }

        if (other.gameObject.layer == 9)
        {
            tar.transform.GetComponent<PlayerController>().returnBoomerang = true;
        }
    }
}
