using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS SCRIPT IS USED TO POPULATE THE LEVELS 
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Theme[] themes = null;                                 //STORES ALL THE TYPES OF THEMES AVAILABLE IN THE GAME
    private int[] tileTypes;                                                //ARRAY USED TO TAKE THE ENVIROMENT TILES FROM THE THEME AND ASSIGN THEM VALUES. THEN ADDS 4 MORE SLOTS FOR CONSISTENT TILES
    int currentTheme;                                                       //USED TO STORE CURRENT THEME PICKED FOR LEVEL

    int pathType;                                                           //USED TO ASSIGN VALUES TO THE TILES DUE TO ENVIRONMENT ARRAY VARYING FROM THEME TO THEME
    int emptyType;                                                          //USED TO ASSIGN VALUES TO THE TILES DUE TO ENVIRONMENT ARRAY VARYING FROM THEME TO THEME
    int startType;                                                          //USED TO ASSIGN VALUES TO THE TILES DUE TO ENVIRONMENT ARRAY VARYING FROM THEME TO THEME
    int endType;                                                            //USED TO ASSIGN VALUES TO THE TILES DUE TO ENVIRONMENT ARRAY VARYING FROM THEME TO THEME

    private int[,] tiles;                                                   //2D ARRAY  TO GENERATE GRID STORING ALL THE TYPES OF TILES THAT ARE AVAILABLE IN THE THEME CHOSEN     
    public int gridX = 0;                                                   //SIZE OF THE GRID ON X AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    public int gridZ = 0;                                                   //SIZE OF THE GRID ON Z AXIS. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    [SerializeField] Vector3 gridOrigin = Vector3.zero;                     //USED TO CONTROL WHERE YOU WANT THE LEVEL TO BE GENERATED. PROBABLY NOT NEEDED!!!!!!!!!!!!!!!!!!!!!!!!
    public float gridSpacingOffset = 2f;                                    //OFSETS THE TILES BY THEIR SIZE. LEFT PUBLIC SO THAT THE CAMERAGIMBAL CAN LOCATE THE CENTER POINT OF GRID
    

    public int pathLength = 0;                                              //HOW MANY TILES THE PATH HAS LOOOOOOLL
    private int pathCount = 0;                                              //USED TO KEEP TRACK OF HOW MANY PATH TILES ITS PLACED ON THE GRID SO FAR
    [SerializeField] GameObject emptyTile = null;                           //THIS IS THE EMPTY TILE USED TO MOVE OTHER TILES
    bool isEmptySpace = false;                                              //USED TO DETERMINE IF A TILE HAS BEEN REPLACED WITH EMPTY TILE


    enum Direction {Top, Bottom, Left, Right};
    Direction startDirection;
    Direction endDirection;

    private void Start()
    {
        tiles = new int[gridX, gridZ];
        GenerateGrid();
    }


    public void GenerateGrid()
    {

        GenerateTheme();                                                    //PICK A THEME TO USE FOR THE LEVEL 
        RandomizeGrid();                                                    //ASSIGN GRID POINTS DIFFERENT TILE TYPES
        StartEndPoint();                                                    //ASSIGN START AND END TILE POSITION
        CreatePath(pathType);                                               //REPLACE SELECTED AMOUNT OF ENVIRONMENT TILES WITH PATH TILES
        EmptySpace();                                                       //DELETE 1 TILE THAT IS NOT A PATH TILE
        SpawnGrid();                                                        //SPAWN THE GRID ONCE EVERYTHING IS IN PALCE
        isEmptySpace = false;                                               //RESET EMPTY TILE TO FALSE FOR NEXT LEVEL
        pathCount = 0;                                                      //RESET pathCount FOR NEXT LEVEL
    }

    private void GenerateTheme()
    {
        currentTheme = Random.Range(0, themes.Length);
        tileTypes = new int[themes[currentTheme].Environment.Length];
        pathType = tileTypes.Length + 1;
        emptyType = tileTypes.Length + 2;
        startType = tileTypes.Length + 3;
        endType =  tileTypes.Length + 4;
    }
    
    private void RandomizeGrid()
    {
        for (int x = 0; x < gridX; x++)                                     //LOOP THROUGH X AXIS ON GRID
        {
            for (int z = 0; z < gridZ; z++)                                 //LOOP THROUGH EACH Z AXIS ON SELECTED X SLOT 
            {
                tiles[x, z] = Random.Range(0, tileTypes.Length);            //ASSIGN THE TILE A VALUE FROM ENVIRONMENT GRID
            }
        }
    }

    private void StartEndPoint()
    {
        int dir = Random.Range(0,2);
        if (dir == 0)
        {
            tiles[Random.Range(0, gridX - 1), 0] = startType;               //START BOTTOM
            startDirection = Direction.Bottom; 
            tiles[Random.Range(0, gridX - 1), (gridZ - 1)] = endType;       //END TOP
            endDirection = Direction.Top;
        }
        else
        {
            tiles[0, Random.Range(0, gridZ - 1)] = startType;               //START LEFT
            startDirection = Direction.Left;
            tiles[(gridX - 1), Random.Range(0, gridZ - 1)] = endType;       //END RIGHT
            endDirection = Direction.Right;
        }
    } 

    private void CreatePath(int pathType)
    {
        while (pathCount < pathLength)                                      //WHILE THE AMOUNT OF pathTiles ASSIGNED TO GRID IS LESS THAN THE pathLength REQUESTED
        {
            int randX = Random. Range(0, gridX);                            //GENERATE RANDOM NUMBER FOR AXIS
            int randZ = Random.Range(0, gridZ);                             //GENERATE RANDOM NUMBER FOR AXIS

            if (tiles[randX, randZ] < tileTypes.Length)                     //CHECK TILE AND IF IT'S NOT A PATH TILE
            {
                tiles[randX, randZ] = pathType;                             
                pathCount++;
            }
        }
    }

    private void EmptySpace()
    {
        while (!isEmptySpace)                                               //AS LONG AS A TILE HAS NOT BEEN REPLACED WITH AN EMPTY TILE
        {
            int randX = Random.Range(0, gridX);                             //CHECK RANDOM ARRAY-X SLOTS
            int randZ = Random.Range(0, gridZ);                             //CHECK RANDOM ARRAY-Z SLOTS

            if (tiles[randX,randZ] < tileTypes.Length)                      //IF ITS NOT A PATH TILE, REPLACE IT WITH AN EMPTY SLOT
            {
                tiles[randX, randZ] = emptyType;
                isEmptySpace = true;
            }
        }
    }

    private void SpawnGrid()          
    {
        for (int x = 0; x < gridX; x++)                                                                                                 //LOOP THROUGH EACH X AXIS
        {
            for (int z = 0; z < gridZ; z++)                                                                                             //LOOP THROUGH EACH Z AXIS ON CURRENT X AXIS
            {
                GameObject tile;                                                                                                        //MAKE GameObject NAMED TILE 
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, transform.position.y, z * gridSpacingOffset) + gridOrigin;   //ASSIGN ITS LOCATION & ACCOUNT FOR OFFSET
                
                
                //!!!!!!!!!!!!!!!!!!!!!!!!!MAKE SURE IT HAS CHECK CONDITION IF INCASE TILE DOESNT HAVE A TILE TYPE!!!!!!!!!!!!!!!!

                if (tiles[x, z] == startType && startDirection == Direction.Bottom)                                                     //IF ITS StartType && BOTTOM
                {
                    tile = Instantiate(themes[currentTheme].startTile, spawnPosition, Quaternion.Euler(0,-180,0));
                    tile.GetComponent<TileType>().direction = TileType.TileDirection.bottom;
                }

                else if (tiles[x, z] == startType && startDirection == Direction.Left)                                                  //IF ITS StartType && LEFT
                {
                    tile = Instantiate(themes[currentTheme].startTile, spawnPosition, Quaternion.Euler(0, -90, 0));
                    tile.GetComponent<TileType>().direction = TileType.TileDirection.left;
                }
                
                else if (tiles[x, z] == endType && endDirection == Direction.Top)                                                       //IF ITS EndType && TOP
                {
                    tile = Instantiate(themes[currentTheme].endTile, spawnPosition, Quaternion.Euler(0,0,0));
                    tile.GetComponent<TileType>().direction = TileType.TileDirection.top;
                }
                                                                                                                                        
                else if (tiles[x, z] == endType && endDirection == Direction.Right)                                                     //IF ITS EndType && RIGHT
                {
                    tile = Instantiate(themes[currentTheme].endTile, spawnPosition, Quaternion.Euler(0, 90, 0));
                    tile.GetComponent<TileType>().direction = TileType.TileDirection.right;
                }

                
                else if (tiles[x,z] == pathType)                                                                                        //IF ITS PathType, SPAWN PathTile
                    tile = Instantiate(themes[currentTheme].pathTile, spawnPosition, Quaternion.identity);                                 

                
                else if(tiles[x,z] == emptyType)                                                                                        //IF ITS EmptyType, SPAWN EmptyTile
                {
                    tile = Instantiate(emptyTile, spawnPosition, Quaternion.identity);
                    tile.transform.parent = transform;
                    tile.transform.SetAsFirstSibling();
                }
                

                else                                                                                                                    //IF ITS AN ENVIRONMENT TILE, SPAWN THE ONE IN THAT SLOT
                    tile = Instantiate(themes[currentTheme].Environment[tiles[x, z]], spawnPosition, Quaternion.identity);

                tile.transform.parent = transform;                                                                                      //MAKE IT A CHILD OF THE LEVEL
            }
        }
    }


    public void ClearGrid()                             //DELETE THE WHOLE LEVEL
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }



    public void PathOn()
    {
        foreach(PathTile pT in this.GetComponentsInChildren<PathTile>())
        {   
            pT.enabled = true;
        }
    }
}
