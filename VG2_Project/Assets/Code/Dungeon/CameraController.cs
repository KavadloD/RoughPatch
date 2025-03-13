using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    //Outlets
    public PlayerController _PlayerController;
    
    //State Tracking
    private float panDuration;

    void Awake()
    {
        panDuration = _PlayerController.panDuration;
    }
    
    public void PanUp()
    {
        StartCoroutine(PanUpCoroutine());
        //print("Pan Up);
    }

    public void PanDown()
    {
        StartCoroutine(PanDownCoroutine());
        //print("Pan down");
    }
    
    public void PanRight()
    {
        StartCoroutine(PanRightCoroutine());
        //print("Pan right");
    }
    
    public void PanLeft()
    {
        StartCoroutine(PanLeftCoroutine());
        //print("Pan left");
    }
    
    //Each coroutine pans the camera in a certain direction
    IEnumerator PanUpCoroutine()
    {
        _PlayerController.isActive = false;
        Tween panUp = transform.DOMove(transform.position + (new Vector3(0, 10, 0)), panDuration);
        yield return panUp.WaitForCompletion();
        _PlayerController.isActive = true;
    }
    
    IEnumerator PanDownCoroutine()
    {
        _PlayerController.isActive = false;
        Tween panDown = transform.DOMove(transform.position + (new Vector3(0, -10, 0)), panDuration);
        yield return panDown.WaitForCompletion();
        _PlayerController.isActive = true;
    }
    
    IEnumerator PanRightCoroutine()
    {
        _PlayerController.isActive = false;
        Tween panRight = transform.DOMove(transform.position + (new Vector3(18, 0, 0)), panDuration);
        yield return panRight.WaitForCompletion();
        _PlayerController.isActive = true;
    }
    
    IEnumerator PanLeftCoroutine()
    {
        _PlayerController.isActive = false;
        Tween panLeft = transform.DOMove(transform.position + (new Vector3(-18, 0, 0)), panDuration);
        yield return panLeft.WaitForCompletion();
        _PlayerController.isActive = true;
    }
}
