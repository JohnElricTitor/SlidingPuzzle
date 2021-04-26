using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SWAPS THE TOUCHED TILE WITH THE EMPTY TILE 
public class Movement : MonoBehaviour
{
    [SerializeField] Transform tower = null;
    [SerializeField] float rayLength = 0;                                                   //LENGTH OF RAYCAST FROM SCREEN
    Vector3 prevPos;                                                                         //zPOS OF TOUCHED TILE
    GameObject emptyTile = null;

    
    bool moving = false;
    Transform movingTile = null;
    Vector3 dest;
    float speed = .2f;

    private void Start()
    {
        enabled = false;
    }

    private void OnEnable()
    {
       EventManager.OnClicked += TouchToWorldPos;
    }

    private void OnDisable()
    {
        EventManager.OnClicked -= TouchToWorldPos;
    }
    
    void FixedUpdate()
    {
        if(moving)
        {
            MoveTile();
        }   
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
                    prevPos = hit.transform.position;                                                                 //STORE CURRENT POS OF HIT OBJECT  
                    movingTile = hit.transform; 
                    dest = emptyTile.transform.position;                                                                
                    moving = true;
                    emptyTile.transform.position = prevPos;                                            //SWAP THE EMPTY TILES POSITION WITH THE HIT OBJECT POSITION
                    
                    EventManager.OnClicked -= TouchToWorldPos;
                }       
            }
        
    }

    private void MoveTile()
    {
        //Moves a step towards dest
        movingTile.position = Vector3.MoveTowards(movingTile.position, dest, speed);

        if(movingTile.position == dest)
        {
            //Stop moving
            moving = false;
            movingTile.position = dest;
            //Call detect methods
            EventManager.eventManager.TileMovement();
            EventManager.OnClicked += TouchToWorldPos;
        }
    }
}
