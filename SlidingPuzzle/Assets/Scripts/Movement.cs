using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SWAPS THE TOUCHED TILE WITH THE EMPTY TILE 
public class Movement : MonoBehaviour
{
    [SerializeField] Transform tower = null;
    [SerializeField] float rayLength = 0;                                                   //LENGTH OF RAYCAST FROM SCREEN
    float xTemp;                                                                            //xPOS OF TOUCHED TILE
    float zTemp;                                                                            //zPOS OF TOUCHED TILE
    GameObject emptyTile = null;


    private void OnEnable()
    {
       EventManager.OnClicked += TouchToWorldPos;
    }

    private void OnDisable()
    {
        EventManager.OnClicked -= TouchToWorldPos;
    }

    private void TouchToWorldPos()                                                                                  //USED TO DETECT TILE SELECTED WHEN YOU TOUCH THE SCREEN
    {                                                                                                               
        RaycastHit hit;                                                                                             //STORE WHAT THE RAYCASTS HITS 
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);                                     //POINT TOUCHED ON SCREEN CONVERTED TO WORLD POS????
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                                //FORWARD DIRECTION FROM CAMERA
                                                                                                                    
        Debug.DrawRay(clickPos, Camera.main.transform.forward * rayLength, Color.red);                              //DRAWS THE RAYCAST FROM POINT TOUCHED ON SCREEN
        
        
        if (Physics.Raycast(ray, out hit, rayLength))                                                               //IF THE RAYCASTS HITS SOMETHING
        {
            emptyTile = tower.GetChild(0).GetChild(0).gameObject;                                          //FIND THE EMPTY GAMEOBJECT IN THE LEVEL
            Debug.DrawLine(clickPos, hit.transform.position , Color.blue);                                          //DRAW BLUE RAYCAST FROM CAMERA TO THE HIT OBJECT

            if (Vector3.Distance(hit.transform.position, emptyTile.transform.position) == 2 && hit.transform.parent.GetSiblingIndex() == 0)  //IF THE HIT OBJECT IS NEAR THE EMPTY OBJECT AND IS THE MAIN LEVEL
            {
                xTemp = hit.transform.position.x;                                                                   //STORE CURRENT xPOS OF HIT OBJECT  
                zTemp = hit.transform.position.z;                                                                   //STORE CURRENT xPOS OF HIT OBJECT  
                hit.transform.position = new Vector3(emptyTile.transform.position.x, 0, emptyTile.transform.position.z);    //SWAP HIT OBJECTS POSITION WITH THE EMPTY TILE POSITION
                emptyTile.transform.position = new Vector3(xTemp, 0, zTemp);                                            //SWAP THE EMPTY TILES POSITION WITH THE HIT OBJECT POSITION
            }                
        }
    }
}
