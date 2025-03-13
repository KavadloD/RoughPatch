using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bridge : MonoBehaviour
{
    //Outlets
    public SpriteRenderer sprite;
    public BoxCollider2D col;
    public GameObject button;
    public GameObject bridgePiece;
    private Vector3 spawnLocation;
    public float bridgeTime;
    
    //State Tracking
    private bool spawnedBridge;

    void Start()
    {
        spawnLocation = new Vector3(-21.5f, 10f, 0f);
        spawnedBridge = false;
    }
    private void Update()
    {
        if (button.GetComponent<Button>().isOn == true)
        {
            if (!spawnedBridge)
            {
                spawnedBridge = true;
                StartCoroutine(CreateBridge());
                col.enabled = false;
            }
        }
        else
        {
            sprite.enabled = false;
            col.enabled = true;
        }
    }

    IEnumerator CreateBridge()
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(bridgePiece, spawnLocation, Quaternion.identity);

            spawnLocation += new Vector3(1, 0, 0);

            yield return new WaitForSeconds(bridgeTime);
        }
    }
}
