using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    
    private int[,] tiles;                   //2D ARRAY  TO GENERATE GRID STORING ALL THE TYPES OF TILES     
    [SerializeField] GameObject[] tileTypes = null;
    public int gridX = 0;
    public int gridZ = 0;
    public float gridSpacingOffset = 2f;
    public Vector3 gridOrigin = Vector3.zero;

    [SerializeField] int pathLength = 0;
    private int pathCount = 0;
    bool isDeadSpace = false;

    [SerializeField] GameObject empty = null;

    private void Start()
    {
        tiles = new int[gridX,gridZ];
        GenerateGrid();
        SpawnGrid();
        EmptySpace();
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

    void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                GameObject clone = Instantiate(tileTypes[tiles[x,z]], spawnPosition, Quaternion.identity);

                clone.transform.parent = this.transform;
            }
        }
    }
    

    void EmptySpace()
    {
        while(!isDeadSpace)
        {
            int rand = Random.Range(0, transform.childCount);
            if (transform.GetChild(rand).GetComponent<TileType>().type != 0)
            {
                GameObject blank = Instantiate(empty, transform.GetChild(rand).position, Quaternion.identity);
                blank.transform.parent = this.transform;
                Destroy(transform.GetChild(rand).gameObject);
                isDeadSpace = true;
            }

        }
    }
}
