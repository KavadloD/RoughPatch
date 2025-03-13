using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomController : MonoBehaviour
{ 
    //Outlets
    public GameObject _camCol;

    //Before the game starts
    void Awake()
    {
        _camCol.SetActive(false);
    }
}
