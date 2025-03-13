using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatesBehavior : MonoBehaviour
{
    //State Tracking
    public float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 3f)
        {
            _timer = 0f;
            Destroy(gameObject);
        }
    }
}
