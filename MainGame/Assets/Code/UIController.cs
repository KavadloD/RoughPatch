using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Outlets
    public GameObject player;
    public Image healthBar;

    //State Tracking
    public float playerHealth;

    public void Awake()
    {
        playerHealth = player.GetComponent<PlayerController>().health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (player.GetComponent<PlayerController>().health / 6);
    }
}
