using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int[,] tiles;                               //2D ARRAY  TO GENERATE GRID STORING ALL THE TYPES OF TILES     
    [SerializeField] Theme tileTypes = null;            //STORES ALL THE TYPES OF TILE THAT WILL SPAWN THROUGH THE INSPECTOR
    
    public int gridZ = 0;                               //SIZE OF THE GRID ON Z AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    public int gridX = 0;                               //SIZE OF THE GRID ON X AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID


    public float gridSpacingOffset = 2f;                //OFSETS THE TILES BY THEIR SIZE. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    [SerializeField] Vector3 gridOrigin = Vector3.zero; //NOT SURE WHAT THIS DOES LOL. GET SOME SLEEP THEN CHECK IT OUT YOU FUCK

    [SerializeField] int pathLength = 0;                //HOW MANY TILES THE PATH HAS LOOOOOOLL
    private int pathCount = 0;                          //USED TO KEEP TRACK OF HOW MANY PATH TILES ITS PLACED ON THE GRID SO FAR
    bool isDeadSpace = false;

    [SerializeField] GameObject empty = null;           //THIS IS THE EMPTY TILE USED TO MOVE OTHER TILES

    private void Start()
    {
        tiles = new int[gridX, gridZ];
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                tiles[x, z] = Random.Range(1, tileTypes.Environment.Length);
            }
        }
        CreatePath();
        pathCount = 0;
        SpawnGrid();
        EmptySpace();
        isDeadSpace = false;
    }

    public void ClearGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void CreatePath()
    {
        while (pathCount < pathLength)          //while the amount of pathTiles spawned is less than the pathLength
        {
            int randX = Random.Range(0, gridX); //Get a random number
            int randZ = Random.Range(0, gridZ); //Get a random number

            if (tiles[randX, randZ] != 0)        //Check a tile and if it's not of type pathTile
            {
                tiles[randX, randZ] = 0;
                pathCount++;
            }
        }
    }

    private void SpawnGrid()
    {

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                GameObject clone;
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, transform.position.y, z * gridSpacingOffset) + gridOrigin;
                if(tiles[x,z] == 0)
                    clone = Instantiate(tileTypes.pathTile, spawnPosition, Quaternion.identity);
                else
                    clone = Instantiate(tileTypes.Environment[tiles[x, z]], spawnPosition, Quaternion.identity);
                clone.transform.parent = transform;
            }
        }
    }


    private void EmptySpace()
    {
        while (!isDeadSpace)
        {
            int rand = Random.Range(0, transform.childCount);
            if (transform.GetChild(rand).GetComponent<TileType>().type != 0)
            {
                GameObject blank = Instantiate(empty, transform.GetChild(rand).position, Quaternion.identity);
                blank.transform.parent = transform;
                Destroy(transform.GetChild(rand).gameObject);
                isDeadSpace = true;
            }

        }
    }
}
