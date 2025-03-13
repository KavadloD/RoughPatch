using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Outlets
    public float projectileSpeed;
    private Vector3 startPosition;
    public float maxDistance;

    void Start()
    {
        startPosition = transform.position;
    }
        
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -1, 0) * (projectileSpeed * Time.deltaTime);
        transform.Rotate(0.0f, 0.0f, 5.0f, Space.Self);
        
        if (Vector2.Distance(transform.position, startPosition) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
