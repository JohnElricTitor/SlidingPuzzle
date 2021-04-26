using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS SCRIPT SIMPLY CONTROLS THE CAMERA ALLOWING YOU TO ROTATE IT AROUND THE CAMERA GIMBAL 
public class CameraController : MonoBehaviour
{

    private Vector3 prevPos;                                                                        //STORE THE PREVIOUS POSITION OF THE CAMERA

    private void Start()
    {
        enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            prevPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        
        if (Input.GetMouseButton(0) && Input.mousePosition.y < Screen.height / 4)
            CameraTurn();
    }

    void CameraTurn()
    { 
        Vector3 direction = prevPos - Camera.main.ScreenToViewportPoint(Input.mousePosition);  //DIRECTION = THE CAMERAS CURRENT POSITION - DIRECTION OF MOUSE 
        transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);               //ROTATE CAMERA ON Y AIX BY NEGATIVE X DIRECTION IN WORLD SPACE
        prevPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);                      //UPDATE THE prevPos THE CAMERAS POSITION;
    }
}
