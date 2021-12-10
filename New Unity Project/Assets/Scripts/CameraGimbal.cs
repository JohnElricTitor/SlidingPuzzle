using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS SCRIPT CENTERS THE CAMERA GIMBAL OBJECT SO THAT THE CAMERA ALWAYS ROTATES AROUND THE CENTER OF THE LEVEL
public class CameraGimbal : MonoBehaviour
{
    //HAVE THIS BE CALLED WHEN LEVEL IS SOLVED AS OPPOSED TO WHEN THE GAME STARTS THAT WAY THE CAMERA RELOCATES DEPENDING ON THE SIZE OF THE CURRENT LEVEL 

    [SerializeField] Transform tower = null;
    int width;                                                                              //STORE THE WIDTH OF THE LEVEL
    int height;                                                                             //STORE THE HEIGHT OF THE LEVEL
    float offSet;                                                                           //STORE THE OFFSET 
   
    // PLACE CAMERA IN CENTER OF THE GRID 
    void Start()
    {
        if (tower != null && tower.GetChild(0).GetComponent<LevelGenerator>() != null)      //USES IF STATEMENT TO PREVENT ERROR FROM MISSING PREFABS
        {
            width = tower.GetChild(0).GetComponent<LevelGenerator>().gridX;
            height = tower.GetChild(0).GetComponent<LevelGenerator>().gridZ;
            offSet = tower.GetChild(0).GetComponent<LevelGenerator>().gridSpacingOffset;
        }
        else
            width = height = 0;

        transform.position = new Vector3(((width * offSet) - offSet) / 2 , 0, ((height * offSet) - offSet) / 2);
    }
}
