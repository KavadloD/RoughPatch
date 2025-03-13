using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBenemy2Controller : MonoBehaviour
{
    public float BB2Health;

    Animator _BB2anim;
    Rigidbody2D _BB2rb;


    // Start is called before the first frame update
    void Start()
    {
        _BB2anim = GetComponent<Animator>();
        _BB2rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float movementSpeed = _BB2rb.velocity.sqrMagnitude;
        _BB2anim.SetFloat("Speed", movementSpeed);
        if (movementSpeed > 0.1f)
        {
            _BB2anim.SetFloat("movementX", _BB2rb.velocity.x);
            _BB2anim.SetFloat("movementY", _BB2rb.velocity.y);
        }

        if (BB2Health < 1)
        {
            _BB2anim.SetTrigger("BBdefeat");
        }
    }
}
