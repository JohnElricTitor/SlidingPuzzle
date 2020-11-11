using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    GameObject empty = null;    //STORE EMPTY TILE HERE 
    float xTemp;                //xPOS OF TOUCHED TILE
    float zTemp;                //zPOS OF TOUCHED TILE

    [SerializeField] float rayLength = 0;   //LENGTH OF RAYCAST FROM SCREEN


    private void Update()
    {
        TouchToWorldPos();
    }

    private void TouchToWorldPos() //USED TO DETECT TILE SELECTED WHEN YOU TOUCH THE SCREEN
    {
        RaycastHit hit;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     //POINT TOUCHED ON SCREEN
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                //FORWARD DIRECTION FROM CAMERA

        Debug.DrawRay(mousePos, Camera.main.transform.forward * rayLength, Color.red); //DRAWS THE RAYCAST FROM POINT TOUCHED ON SCREEN

        if (Input.GetMouseButtonUp(0))
        {
            GameObject empty = GameObject.Find("Empty(Clone)");

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (Vector3.Distance(hit.transform.position, empty.transform.position) == 2)
                {
                    xTemp = hit.transform.position.x;
                    zTemp = hit.transform.position.z;
                    hit.transform.position = new Vector3(empty.transform.position.x, 0, empty.transform.position.z);
                    empty.transform.position = new Vector3(xTemp, 0, zTemp);
                }
            }
        }

    }
}
