using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//THIS SCRIPT CONTROLS THE TOWER TO GENERATE ALL THE FLOORS AND MOVE THEM WHEN THE LEVEL CURRENT LEVEL IS COMPELTED
public class TowerController : MonoBehaviour
{
    [SerializeField] int numberOfLevels;                //THE AMOUNT OF LEVELS IN THE TOWER
    [SerializeField] int levelOffset = 2;               //THE DISTANCE BETWEEN EACH LEVEL               
    [SerializeField] GameObject levelPrefab = null;     //PREFAB HOLDING EACH LEVEL   


    private void OnEnable()
    {
        EventManager.OnUI += NextLevel;
        EventManager.OnUI += MoveTower;
    }

    private void OnDisable()
    {
        EventManager.OnUI -= NextLevel;
        EventManager.OnUI -= MoveTower;
    }

    void Start()
    {
        GenerateTower();
    }

    //CREATE ALL THE LEVELS AS CHILDREN GAME OBJECTS
    void GenerateTower()
    {
        for (int y = 0; y < numberOfLevels; y++)                                                                    //LOOP FOR GOING FLOOR BY FLOOR
        {
            GameObject level;
            
            if (levelPrefab != null)
                level = levelPrefab;
            else
            {
                level = new GameObject("level");
                Debug.LogWarning("Tower missing Level prefab");
            }
            
            level = Instantiate(level, transform.position, Quaternion.identity);                                    //INSTANTIATE FLOOR LEVEL
            level.name = ("Level " + (y + 1));                                                                      //NAME IT ACCORDING TO WHAT LEVEL THE PLAYER IS ON
            level.transform.parent = transform;                                                                     //PARENT THE LEVEL GAMEOBJECTS TO THE TOWER
            level.transform.position = new Vector3(transform.position.x, y * - levelOffset, transform.position.z);  //LOWER LEVELS BY DESIRED OFFSET
        }
    }

    //DROPS CURRENT LEVEL TO BOTTOM OF PARENT AND RAISES THE REST OF THE TOWER UP BY yOFFSET
    void NextLevel()
    {
        Transform level = transform.GetChild(0);                                                                    //STORE THE TRANSFORM OF THE CURRENT LEVEL
        level.GetComponent<LevelGenerator>().ClearGrid();                                                           //UNLOAD CURRENT LEVEL
        level.position = new Vector3(0, transform.GetChild(transform.childCount - 1).position.y - levelOffset, 0);  //CHANGE CURRENT LEVELS POSITION TO THAT OF THE LAST CHILD IN ARRAY
        level.GetComponent<LevelGenerator>().GenerateGrid();                                                        //LOAD IN NEW LEVEL BEFORE CHANGING ITS INDEX
        numberOfLevels += 1;                                                                                        //INCREASE THE LEVELS COUNT
        level.name = ("Level " + (numberOfLevels));                                                                 //RENAME THIS LEVEL
        level.SetAsLastSibling();                                                                                   //CHANGE ITS INDEX TO THE LAST 
    }
        
    void MoveTower()
    {
        for (int i = 0; i < transform.childCount; i++)                                                              //LOOP THROUGH THE REST OF THE LEVELS AND MOVE THEM UP BY levelOffset
            transform.GetChild(i).transform.position = new Vector3 (0, transform.GetChild(i).position.y + levelOffset, 0);
    }
}
