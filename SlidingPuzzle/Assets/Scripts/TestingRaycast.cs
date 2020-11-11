using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Direction 
{ 
    public Mesh tileUD;
    public Mesh tileLR;
    public Mesh tileDL;
    public Mesh tileDR;
    public Mesh tileUL;
    public Mesh tileUR;
}

public class TestingRaycast : MonoBehaviour
{
    
    [SerializeField] Direction directions;  //STORE ALL VARIATIONS OF WALKING TILE. USED A SEPERATE CLASS SO THAT UNITYS INTERACE SPECIFIES WHAT TILE TO PLACE IN WHAT SLOT (CONSIDER USING A STRUCT AFTER YOU FIX THE TILE NAMES IN BLENDER)
    [SerializeField] float rayLength = 0;   //LENGTH OF THE 4 RAYCASTS USED TO DETECT TILES
    [SerializeField] float offSet = 0;      //CURRENTLY NOT BEING USED BUT INCASE YOU WANT TO OFFSET THE RAYCASTS SO THEY DONT START AT CENTER OF OBJECT

    Ray rayUp, rayDown, rayLeft, rayRight;  //4 DIRECTIONAL ARRAYS
    bool up, down, left, right;             //BOOLEANS TO FOR ANY SIDE THAT HITS A WALKABLE TILE
    MeshFilter currentMesh;                 //USED TO STORE CURRENT DIRECTION MESH OF TILE

    private void Start()
    {
        currentMesh = GetComponent<MeshFilter>();   //GET THE CURRENT MESH SO YOU CAN CHANGE IT 
    }
    void Update()
    {
        DetectTile();
        TileChange();
    }


    private void DetectTile() //FUNCTION FOR DETECTING THE TILES ON ALL 4 DIRECTIONS USING RAYCASTS  
    {
        //SET RAYS OF ALL 4 DIRECTIONS
        rayUp = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), Vector3.forward); 
        rayDown = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - offSet), Vector3.back); 
        rayLeft = new Ray(new Vector3(transform.position.x - offSet, transform.position.y, transform.position.z), Vector3.left); 
        rayRight = new Ray(new Vector3(transform.position.x + offSet, transform.position.y, transform.position.z), Vector3.right);
     
        RaycastHit hit;

        //RAYCAST UP
        if (Physics.Raycast(rayUp, out hit, rayLength)) 
        {
            //IF IT HITS WALKING TILE THEN IT DRAWS A RED RAY AND TURNS UP TO TRUE. LOGS THE NAME OF TILE IT HITS
            if (hit.transform.GetComponent<TileType>().type == 0)
            { 
                up = true;
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), hit.point, Color.red);
                Debug.Log("Up Raycast hit: " + hit.transform.gameObject.name);
            }
            //ELSE KEEP FALSE AND DRAW BLACK RAY
            else
            {
                up = false;
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), Vector3.forward * rayLength, Color.black); //Up
            }
        }

        //RAYCAST DOWN 
        if (Physics.Raycast(rayDown, out hit, rayLength))
        {
            //IF IT HITS WALKING TILE THEN IT DRAWS A RED RAY AND TURNS DOWN TO TRUE. LOGS THE NAME OF TILE IT HITS
            if (hit.transform.GetComponent<TileType>().type == 0)
            {
                down = true;
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), hit.point, Color.red);
                Debug.Log("Down Raycast hit: " + hit.transform.gameObject.name);
            }
            //ELSE KEEP FALSE AND DRAW BLUE RAY
            else
            {
                down = false;
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - offSet), Vector3.back * rayLength, Color.blue); //Down
            }
        }

        //RAYCAST LEFT
        if (Physics.Raycast(rayLeft, out hit, rayLength))
        {
            //IF IT HITS WALKING TILE THEN IT DRAWS A RED RAY AND TURNS LEFT TO TRUE. LOGS THE NAME OF TILE IT HITS
            if (hit.transform.GetComponent<TileType>().type == 0)
            {
                left = true;
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), hit.point, Color.red);
                Debug.Log("Left Raycast hit: " + hit.transform.gameObject.name);
            }
            //ELSE KEEP FALSE AND DRAW GREEN RAY
            else
            {
                left = false;
                Debug.DrawRay(new Vector3(transform.position.x - offSet, transform.position.y, transform.position.z), Vector3.left * rayLength, Color.green); //Left
            }
        }

        //RAYCAST RIGHT
        if (Physics.Raycast(rayRight, out hit, rayLength))
        {
            //IF IT HITS WALKING TILE THEN IT DRAWS A RED RAY AND TURNS RIGHT TO TRUE. LOGS THE NAME OF TILE IT HITS
            if (hit.transform.GetComponent<TileType>().type == 0)
            {
                right = true;
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + offSet), hit.point, Color.red);
                Debug.Log("Right Raycast hit: " + hit.transform.gameObject.name);
            }
            //ELSE KEEP FALSE AND YELLOW ORIGINAL RAY
            else
            {
                right = false;
                Debug.DrawRay(new Vector3(transform.position.x + offSet, transform.position.y, transform.position.z), Vector3.right * rayLength, Color.yellow); //Right
            }
        }
    }

    private void TileChange() //CHANGES TILE MESH DEPENDING ON DIRECTIONAL BOOLS
    {
        if(up || down)
        {
            currentMesh.mesh = directions.tileUD;
        }
        if(left || right)
        {
            currentMesh.mesh = directions.tileLR;
        }
        if(up && left)
        {
           currentMesh.mesh = directions.tileUL;
        }
        if(up && right)
        {
            currentMesh.mesh = directions.tileUR;
        }
        if(down && left)
        {
            currentMesh.mesh = directions.tileDL;
        }
        if(down && right)
        {
            currentMesh.mesh = directions.tileDR;
        }
    }
}
