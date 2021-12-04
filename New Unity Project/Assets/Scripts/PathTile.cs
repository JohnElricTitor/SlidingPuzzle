using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System; // this is so you can convert bool to int

[System.Serializable]
public class Direction 
{
    public GameObject Default; 
    public GameObject tileUD;
    public GameObject tileLR;
    public GameObject tileDL;
    public GameObject tileDR;
    public GameObject tileUL;
    public GameObject tileUR;
}

[RequireComponent(typeof(TileType))]
public class PathTile : MonoBehaviour
{
    
    [SerializeField] Direction directions = null;                       //STORE ALL VARIATIONS OF WALKING TILE. USED A STRUCT SO THAT UNITYS INTERACE SPECIFIES WHAT TILE TO PLACE IN WHAT SLOT

    [SerializeField] float rayLength = 0;                               //LENGTH OF THE 4 RAYCASTS USED TO DETECT TILES
    [SerializeField] float rayOffSet = 0;                               //CURRENTLY NOT BEING USED BUT INCASE YOU WANT TO OFFSET THE RAYCASTS SO THEY DONT START AT CENTER OF OBJECT
    Ray rayUp, rayDown, rayLeft, rayRight;                              //4 DIRECTIONAL ARRAYS
    bool isUp, isDown, isLeft, isRight;                                 //BOOLEANS THAT ARE USED TO STORE WHAT DIRECTIONAL RAYCASTS HITS A PATH TILE 


    public bool wasConnected;
    public bool isConnected;

    public bool isStart;
    public bool isEnd;
    TileType.tileType up,down,left,right;

    private void Start()
    {
        InstatiateTiles();
        DetectTile();

        if(transform.parent.GetSiblingIndex() != 0)
            enabled = false;
    }


    private void OnEnable()
    {
        EventManager.OnTileMovement += DetectTile;
        EventManager.onWinCheck += WinCheck;
    }
    
    private void OnDisable()
    {
        EventManager.OnTileMovement -= DetectTile;
        EventManager.onWinCheck -= WinCheck;
    }

    void InstatiateTiles()                                                                                                              //INSTANTIATE ALL 7 TILES AS CHILDREN OF EMPTY PARENT 
    {
        Instantiate(directions.Default, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileUD, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileLR, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileDL, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileDR, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileUL, transform.position, Quaternion.identity, transform);
        Instantiate(directions.tileUR, transform.position, Quaternion.identity, transform);
        EnableTile(0);
    }


    public void DetectTile()
    {
        rayUp = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOffSet), Vector3.forward);    //RAYCAST FOR UP DIRECTION                      
        rayDown = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - rayOffSet), Vector3.back);     //RAYCAST FOR DOWN DIRECTION
        rayLeft = new Ray(new Vector3(transform.position.x - rayOffSet, transform.position.y, transform.position.z), Vector3.left);     //RAYCAST FOR LEFT DIRECTION
        rayRight = new Ray(new Vector3(transform.position.x + rayOffSet, transform.position.y, transform.position.z), Vector3.right);   //RAYCAST FOR RIGHT DIRECTION

        isUp = CheckDirection(rayUp);                                                                                                   //CHECK IF UP IS CONNECTED TO TILE OF SAME TYPE
        isDown = CheckDirection(rayDown);                                                                                               //CHECK IF DOWN IS CONNECTED TO TILE OF SAME TYPE
        isLeft = CheckDirection(rayLeft);                                                                                               //CHECK IF LEFT IS CONNECTED TO TILE OF SAME TYPE
        isRight = CheckDirection(rayRight);                                                                                             //CHECK IF RIGHT IS CONNECTED TO TILE OF SAME TYPE
        
        ChangeDirection();                                                                                                              //CHANGE DIRECTION OF THE TILE

        up = CheckType(rayUp);
        down = CheckType(rayDown);
        left = CheckType(rayLeft);
        right = CheckType(rayRight);
    
    }

    bool CheckDirection(Ray raycast)                    
    {    
        bool dir = false;                                                                                                               //USED TO RETURN IF THEY RAY HITS PATH OR NOT 

        RaycastHit hit;                                                                                                                 //STORE OBJECT HIT

        if (Physics.Raycast(raycast, out hit, rayLength))                                                                               //HIT RAY HITS OBJECT
        {
            if (hit.transform != transform && hit.transform.GetComponent<TileType>() != null)                                           //AND OBJECT HAS TILETYPE WITH TYPE == TO PATH OR == TO END
            {
                dir = (hit.transform.GetComponent<TileType>().type == TileType.tileType.path || 
                       hit.transform.GetComponent<TileType>().type == TileType.tileType.end || 
                       hit.transform.GetComponent<TileType>().type == TileType.tileType.start) ? true : false;
            }

        }
        return dir;       //RETURN IF ITS TOUCHING 
    }
     

    
    void ChangeDirection()
    {
        if (!isUp && !isDown && !isLeft && !isRight)                                                //DEFAULT 
            EnableTile(0);
        
        else if ((isUp || isDown) && (!isLeft && !isRight))                                         //UD
            EnableTile(1);
        
        else if ((isLeft || isRight) && (!isUp && !isDown))                                         //LR
            EnableTile(2);
        
        else if ((isDown && isLeft) && (!isUp && !isRight))                                         //DL
            EnableTile(3);
        
        else if ((isDown && isRight) && (!isUp && !isLeft))                                         //DR
            EnableTile(4);
        
        else if ((isUp && isLeft) && (!isDown && !isRight))                                         //UL
            EnableTile(5);
        
        else if ((isUp && isRight) && (!isDown && !isLeft))                                         //UR
            EnableTile(6);
        else
            return;
    }
    
    void EnableTile(int enabled)                                                                                                        //DISABLE ALL THE TILES THEN YOU CAN MANUAL TURN ON THE ONE YOU WANT 
    {
        for(int x = 0; x < 7; x++)
        {
            transform.GetChild(x).gameObject.SetActive(false);
        }
        transform.GetChild(enabled).gameObject.SetActive(true);
    }
    
    TileType.tileType CheckType(Ray raycast)
    {
         TileType.tileType isType = TileType.tileType.path;                                                                             //ONLY MADE IT START AS PATH BECAUSE IT SAYS IT REQUIRES A VALUE 
    
         RaycastHit hit;
    
         if(Physics.Raycast(raycast, out hit, rayLength))
         {
             if (hit.transform != transform && hit.transform.GetComponent<TileType>() != null)
                 isType = hit.transform.GetComponent<TileType>().type;
         }
         return isType;
    }

    void CheckPath ()
    {
        isStart = (up == TileType.tileType.start || down == TileType.tileType.start || left == TileType.tileType.start || right == TileType.tileType.start) ? true : false;
        isEnd = (up == TileType.tileType.end || down == TileType.tileType.end || left == TileType.tileType.end || right == TileType.tileType.end) ? true : false;

        if(!isStart && !isEnd)
        {
           // up == TileType.tileType.path || down == TileType.tileType.path || left == TileType.tileType.path || right == TileType.tileType.path;
        
        }
    }

    
    void WinCheck()
    {
        isConnected = (Convert.ToInt32(isUp) + Convert.ToInt32(isDown) + Convert.ToInt32(isRight) + Convert.ToInt32(isLeft) == 2) ? true : false;
        
        if(isConnected && !wasConnected)
        {
            transform.GetComponentInParent<TowerController>().currentCount++;
            wasConnected = true;
        }
        if (!isConnected && wasConnected)
        {
            transform.GetComponentInParent<TowerController>().currentCount--;
            wasConnected = false;
        }
        else
            return;
    }
}
