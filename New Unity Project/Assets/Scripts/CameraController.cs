using UnityEngine;


//THIS SCRIPT SIMPLY CONTROLS THE CAMERA ALLOWING YOU TO ROTATE IT AROUND THE CAMERA GIMBAL AND SWIPE UP TO PAUSE
public class CameraController : MonoBehaviour
{

    private Vector3 startPos;                                                               //STORE THE PREVIOUS POSITION OF THE CAMERA
    private Vector3 endPose;
    private Vector3 direction;

    private Vector3 swipeDir;

    private void Start()
    {
        enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))                                                    //IF BUTTON IS PRESSED
            startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        else if (Input.GetMouseButton(0))                                                   //IF HELD THEN ROTATE 
            CameraTurn();

        else if (Input.GetMouseButtonUp(0))                                                 //IF RELEASE AND ROTATING IS NOT TRUE 
        {
            PauseSwipe();
        }
    }

    void CameraTurn()
    {
        direction = startPos - Camera.main.ScreenToViewportPoint(Input.mousePosition);      //DIRECTION = THE CAMERAS CURRENT POSITION - DIRECTION OF MOUSE 
        transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);            //ROTATE CAMERA ON Y AIX BY NEGATIVE X DIRECTION IN WORLD SPACE
        startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);                  //UPDATE THE prevPos THE CAMERAS POSITION;
    }

    void PauseSwipe()                                                                       //!!!!!!!!!!!!CORRECT SWIPE UP, RIGHT NOW YOU CAN ROTATE INTO A SWIPE!!!!!!!!!
    {
        endPose = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        swipeDir = new Vector3(endPose.x - startPos.x, endPose.y - startPos.y, 0);
        swipeDir.Normalize();
        if (swipeDir.y >= 0.7f)
            EventManager.eventManager.onPause();

       // Debug.Log(swipeDir);
    }
}