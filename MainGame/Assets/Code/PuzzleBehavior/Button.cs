using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Button : MonoBehaviour
{
    //Outlets
    public GameObject _bridge;
    public SpriteResolver _res;
    
    //State Tracking
    public bool isOn;
    
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            _res.SetCategoryAndLabel("one", "1");
        }
        if(!isOn)
        {
            _res.SetCategoryAndLabel("one", "0");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (isOn)
            {
                isOn = false;
            }
            else
            {
                isOn = true;
            }
        }
    }
}
