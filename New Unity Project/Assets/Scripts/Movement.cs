using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT SWAPS THE TOUCHED TILE WITH THE EMPTY TILE 
public class Movement : MonoBehaviour
{   
    [SerializeField] float rayLength = 0;                                                   //LENGTH OF RAYCAST FROM SCREEN

    [SerializeField] Transform tower = null;                                                //USE THIS TO GET THE EMPTY OBJECT ON CURRENT LEVEL
    GameObject emptyTile = null;                                                            //STORE EMPTY TILE 
    bool moving = false;                                                                    //IS THE CURRENT THAT WAS CLICKED ON CURRENTLY MOVING 
    Vector3 dest;
    float speed = .2f;
    

    Transform movingTile = null;
    private Vector3 prevPos;                                                                //zPOS OF TOUCHED TILE


    private void Start()
    {
        enabled = false;                                                                    //DISABLE THIS SCRIPT WHEN GAME STARTS AS IT STARTS IN A PAUSED STATE 
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

    private void TouchToWorldPos()                                                          //USED TO DETECT TILE SELECTED WHEN YOU TOUCH THE SCREEN
    {                                                                                           
        RaycastHit hit;                                                                     //STORE WHAT THE RAYCASTS HITS 
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);             //POINT TOUCHED ON SCREEN CONVERTED TO WORLD POS????
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                        //FORWARD DIRECTION FROM CAMERA
        

        if (Physics.Raycast(ray, out hit, rayLength))                                       //IF THE RAYCASTS HITS SOMETHING
        {
            emptyTile = tower.GetChild(0).GetChild(0).gameObject;                           //FIND THE EMPTY GAMEOBJECT IN THE LEVEL
            Debug.DrawLine(clickPos, hit.transform.position , Color.blue);                  //DRAW BLUE RAYCAST FROM CAMERA TO THE HIT OBJECT
            
            if (Vector3.Distance(hit.transform.position, emptyTile.transform.position) == 2 && hit.transform.parent.GetSiblingIndex() == 0)     //IF THE HIT OBJECT IS NEAR THE EMPTY OBJECT AND IS ON THE MAIN LEVEL
            {
                if (hit.transform.GetComponent<TileType>() != null && hit.transform.GetComponent<TileType>().type == TileType.tileType.end ||   //IF THE TOUCHED TILE IS START OR END TILE
                    hit.transform.GetComponent<TileType>() != null && hit.transform.GetComponent<TileType>().type == TileType.tileType.start)   //IF THE TOUCHED TILE IS START OR END TILE
                {
                    
                    if (TouchingMainTile(hit))                                              //IF THE PLAYER CLICKED ON A MAIN TILE AND THE TILE IS ON THE SAME ROW OR COLUMN AS THE EMPTY 
                    {
                        prevPos = hit.transform.position;                                   //STORE CURRENT POS OF HIT TILE FOR EMPTY TILE TO MOVE THERE AFTER 
                        
                        movingTile = hit.transform;                                         //CURRENT POSITION OF TOUCHED TILE
                        dest = emptyTile.transform.position;                                //DESTINATION = POSITION OF EMPTY TILE
                        moving = true;                              
                        emptyTile.transform.position = prevPos;                             //SWAP THE EMPTY TILES POSITION WITH THE HIT OBJECT POSITION                       
                    }
                    else
                        return;
                }               


                else
                {
                    prevPos = hit.transform.position;                                       //STORE CURRENT POS OF HIT OBJECT  
                    movingTile = hit.transform;
                    dest = emptyTile.transform.position;
                    moving = true;
                    emptyTile.transform.position = prevPos;                                 //SWAP THE EMPTY TILES POSITION WITH THE HIT OBJECT POSITION
                }
                    
                
                EventManager.OnClicked -= TouchToWorldPos;
            }       
        }
    }

    private void MoveTile()
    {
        movingTile.position = Vector3.MoveTowards(movingTile.position, dest, speed);        //MOVE TOUCHED TILE A STEP CLOSER TO EMPTY PER FRAME UNTIL IT REACHES EMPTY 

        if (movingTile.position == dest)                                                    //IF TOUCHED TILE POSITION EQUALS EMPTY TILE POSITION
        {
            //Stop moving
            moving = false;                                                     
            movingTile.position = dest;

            //Call detect methods
            EventManager.eventManager.TileMovement();
            EventManager.OnClicked += TouchToWorldPos;
            EventManager.eventManager.WinCheck();
        }
    }


    
    private bool TouchingMainTile(RaycastHit hit)       //CHECKS IF THE MAIN TILE IS ON THE SAME ROW OR COLUMN AS THE EMPTY SO THAT IT IS NOT SLID INTO THE LEVEL CENTER 
    {    
        if (hit.transform.GetComponent<TileType>().direction == TileType.TileDirection.top && emptyTile.transform.position.z == hit.transform.position.z ||
            hit.transform.GetComponent<TileType>().direction == TileType.TileDirection.bottom && emptyTile.transform.position.z == hit.transform.position.z)     //MAIN TILE IS ON THE SAME ROW OR COLUMN AS EMPTY (BOTTOM OR TOP)
        {
            return true;
        }
        
        if (hit.transform.GetComponent<TileType>().direction == TileType.TileDirection.left && emptyTile.transform.position.x == hit.transform.position.x ||
            hit.transform.GetComponent<TileType>().direction == TileType.TileDirection.right && emptyTile.transform.position.x == hit.transform.position.x)      //MAIN TILE IS ON THE SAME ROW OR COLUMN AS EMPTY (BOTTOM OR TOP)
        {
            return true;
        }
        else
            return false;    
    }
}
