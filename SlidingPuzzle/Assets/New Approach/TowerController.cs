using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] int levelCount;            //The amount of levels in the Tower
    [SerializeField] int yOffset = 2;
    [SerializeField] GameObject levelPrefab;   
   
    
    void Start()
    {
        GenerateTower();
    }

    //CREATE ALL THE LEVELS AS CHILDREN GAME OBJECTS
    void GenerateTower()
    {
        for (int y = 0; y < levelCount; y++)                                                                    //LOOP FOR GOING FLOOR BY FLOOR
        {
            GameObject level = Instantiate(levelPrefab, transform.position, Quaternion.identity);               //INSTANTIATE FLOOR LEVEL
            level.name = ("Level " + (y + 1));                                                                  //NAME IT ACCORDING TO WHAT LEVEL THE PLAYER IS ON
            level.transform.parent = transform;                                                                 //PARENT THE LEVEL GAMEOBJECTS TO THE TOWER
            level.transform.position = new Vector3(transform.position.x, y * - yOffset, transform.position.z);  //LOWER LEVELS BY DESIRED OFFSET
        }
    }

    //DROPS CURRENT LEVEL TO BOTTOM OF PARENT AND RAISES THE REST OF THE TOWER UP BY yOFFSET
    void NextLevel()
    {
        Transform level = transform.GetChild(0);                                                                //STORE THE TRANSFORM OF THE CURRENT LEVEL
        level.GetComponent<LevelGenerator>().ClearGrid();                                                       //UNLOAD CURRENT LEVEL
        level.position = new Vector3(0, transform.GetChild(transform.childCount - 1).position.y - yOffset, 0);  //GET THE LAST CHILDS POSITION AND RELOCATE CURRENT LEVEL TO ITS POSITION
        levelCount += 1;                                                                                        //INCREASE THE LEVELS COUNT
        level.name = ("Level " + (levelCount));                                                                 //RENAME LEVEL
        transform.GetChild(0).GetComponent<LevelGenerator>().GenerateGrid();                                    //LOAD IN NEW LEVEL
        level.SetAsLastSibling();                                                                               //RELOCATE THE LEVEL FROM FIRST SIBLING TO LAST 
    }

    void MoveTower()
    {
        for (int i = 0; i < transform.childCount; i++)                                                          //LOOP THROUGH THE REST OF THE LEVELS AND MOVE THEM UP BY yOFFSET
            transform.GetChild(i).transform.position = new Vector3 (0, transform.GetChild(i).position.y + yOffset, 0);   
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
            MoveTower();
        }
    
    }
}
