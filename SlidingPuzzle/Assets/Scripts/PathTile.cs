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
    
    [SerializeField] Direction directions = null;   //STORE ALL VARIATIONS OF WALKING TILE. USED A STRUCT SO THAT UNITYS INTERACE SPECIFIES WHAT TILE TO PLACE IN WHAT SLOT

    [SerializeField] float rayLength = 0;           //LENGTH OF THE 4 RAYCASTS USED TO DETECT TILES
    [SerializeField] float rayOffSet = 0;           //CURRENTLY NOT BEING USED BUT INCASE YOU WANT TO OFFSET THE RAYCASTS SO THEY DONT START AT CENTER OF OBJECT
    Ray rayUp, rayDown, rayLeft, rayRight;          //4 DIRECTIONAL ARRAYS
    bool up, down, left, right;                     //BOOLEANS THAT ARE USED TO STORE WHAT DIRECTIONAL RAYCASTS HITS A PATH TILE 

    private void Start()
    {
        InstatiateTiles();
        DetectTile();
    }

    private void FixedUpdate()
    {
        if(transform.parent.GetSiblingIndex() == 0)
            DetectTile();
    }
   
    //private void OnEnable()
    //{
    //    EventManager.OnDetect += DetectTile;
    //    //EventManager.OnClicked += ChangeDirection;
    //    EventManager.OnClicked -= WinCheck;
    //}
    //
    //private void OnDisable()
    //{
    //    EventManager.OnDetect -= DetectTile;
    //    //EventManager.OnClicked -= ChangeDirection;
    //    EventManager.OnClicked -= WinCheck;
    //}

    void InstatiateTiles()                                                                  //INSTANTIATE ALL 7 TILES AS CHILDREN OF EMPTY PARENT 
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

        up = CheckDirection(rayUp);                                                                                                     //CHECK IF UP IS CONNECTED TO TILE OF SAME TYPE
        down = CheckDirection(rayDown);                                                                                                 //CHECK IF DOWN IS CONNECTED TO TILE OF SAME TYPE
        left = CheckDirection(rayLeft);                                                                                                 //CHECK IF LEFT IS CONNECTED TO TILE OF SAME TYPE
        right = CheckDirection(rayRight);                                                                                               //CHECK IF RIGHT IS CONNECTED TO TILE OF SAME TYPE
        
        ChangeDirection();                                                                                                              //CHANGE DIRECTION OF THE TILE
        Debug.Log("detected");
    }

    private bool CheckDirection(Ray raycast)                    
    {    
        bool dir = false;                                                                   //USED TO RETURN IF THEY RAY HITS PATH OR NOT 
        RaycastHit hit;                                                                     //STORE OBJECT HIT

        if (Physics.Raycast(raycast, out hit, rayLength))                                   //HIT RAY HITS OBJECT
        {
            if (hit.transform != transform && hit.transform.GetComponent<TileType>() != null &&                           //AND OBJECT HAS TILETYPE WITH TYPE == TO PATH OR == TO END
              (hit.transform.GetComponent<TileType>().type == TileType.tileType.path ||
               hit.transform.GetComponent<TileType>().type == TileType.tileType.end))
            {
                dir = true;                                                                 //CONNECTED IS TRUE
            }
            else
                dir = false;                                                                //CONNECTED IS FALSE
        }
        return dir;                                                                         //RETURN RESULT
    }

    void ChangeDirection()
    {
        if (!up && !down && !left && !right)                                                //DEFAULT 
            EnableTile(0);
        
        else if ((up || down) && (!left && !right))                                         //UD
            EnableTile(1);
        
        else if ((left || right) && (!up && !down))                                         //LR
            EnableTile(2);
        
        else if ((down && left) && (!up && !right))                                         //DL
            EnableTile(3);
        
        else if ((down && right) && (!up && !left))                                         //DR
            EnableTile(4);
        
        else if ((up && left) && (!down && !right))                                         //UL
            EnableTile(5);
        
        else if ((up && right) && (!down && !left))                                         //UR
            EnableTile(6);
        else
            return;
    }


    void EnableTile(int enabled)                                                                     //DISABLE ALL THE TILES THEN YOU CAN MANUAL TURN ON THE ONE YOU WANT 
    {
        for(int x = 0; x < 7; x++)
        {
            transform.GetChild(x).gameObject.SetActive(false);
        }
        transform.GetChild(enabled).gameObject.SetActive(true);
    }

    void WinCheck()
    {
        if (Convert.ToInt32(up) + Convert.ToInt32(down) + Convert.ToInt32(right) + Convert.ToInt32(left) == 2)
            Debug.Log(gameObject.name + " is connected");
        else
        {
            Debug.Log(gameObject.name + " UP= " + Convert.ToInt32(up));
            Debug.Log(gameObject.name + " DOWN= " + Convert.ToInt32(down));
            Debug.Log(gameObject.name + " LEFT= " + Convert.ToInt32(left));
            Debug.Log(gameObject.name + " RIGHT= " + Convert.ToInt32(right));
        }           
    }
}
