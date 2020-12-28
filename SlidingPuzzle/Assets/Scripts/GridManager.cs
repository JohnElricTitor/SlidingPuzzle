using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    
    private int[,] tiles;                               //2D ARRAY  TO GENERATE GRID STORING ALL THE TYPES OF TILES     
    [SerializeField] GameObject[] tileTypes = null;     //STORES ALL THE TYPES OF TILE THAT WILL SPAWN THROUGH THE INSPECTOR
    public int gridZ = 0;                               //SIZE OF THE GRID ON Z AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    public int gridX = 0;                               //SIZE OF THE GRID ON X AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID


    public int gridY = 0;
    public int gridYoffSet;
    int currentLvl = 0;

    public float gridSpacingOffset = 2f;                //OFSETS THE TILES BY THEIR SIZE. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    [SerializeField] Vector3 gridOrigin = Vector3.zero; //NOT SURE WHAT THIS DOES LOL. GET SOME SLEEP THEN CHECK IT OUT YOU FUCK

    [SerializeField] int pathLength = 0;                //HOW MANY TILES THE PATH HAS LOOOOOOLL
    private int pathCount = 0;                          //USED TO KEEP TRACK OF HOW MANY PATH TILES ITS PLACED ON THE GRID SO FAR
    bool isDeadSpace = false;

    [SerializeField] GameObject empty = null;           //THIS IS THE EMPTY TILE USED TO MOVE OTHER TILES
   
    private void Start()
    {
        //gridY = Random.Range(5,25);

        for(int y = 0; y < gridY; y++)
        {
            //gridX = Random.Range(5, 15);
            //gridZ = Random.Range(5, 15);
            tiles = new int[gridX,gridZ];
            
            currentLvl = y;
            
            GameObject level = new GameObject("Level " + (currentLvl + 1));
            level.transform.parent = this.transform;
            level.transform.position = new Vector3(transform.position.x, currentLvl * -gridYoffSet, transform.position.z);
            
            GenerateGrid();
            pathCount = 0;

            SpawnGrid(level.transform);
            EmptySpace(level.transform);
            isDeadSpace = false;
        }
    }

    void GenerateGrid() 
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                tiles[x, z] = Random.Range(1, tileTypes.Length);
            }
        }
        CreatePath();
    }

    void CreatePath()
    {
        while (pathCount < pathLength)          //while the amount of pathTiles spawned is less than the pathLength
        {
            int randX = Random.Range(0, gridX); //Get a random number
            int randZ = Random.Range(0, gridZ); //Get a random number

            if (tiles[randX,randZ] != 0)        //Check a tile and if it's not of type pathTile
            {
                tiles[randX, randZ] = 0;
                pathCount++;
            }
        }
    }

    void SpawnGrid(Transform level)
    {

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, currentLvl * -gridYoffSet, z * gridSpacingOffset) + gridOrigin;
                GameObject clone = Instantiate(tileTypes[tiles[x,z]], spawnPosition, Quaternion.identity);

                clone.transform.parent = level.transform;
            }
        }
    }
    

    void EmptySpace(Transform level)
    {
        while(!isDeadSpace)
        {
            int rand = Random.Range(0, level.childCount);
            if (level.GetChild(rand).GetComponent<TileType>().type != 0)
            {
                GameObject blank = Instantiate(empty, level.GetChild(rand).position, Quaternion.identity);
                blank.transform.parent = level;
                Destroy(level.GetChild(rand).gameObject);
                isDeadSpace = true;
            }

        }
    }
}
