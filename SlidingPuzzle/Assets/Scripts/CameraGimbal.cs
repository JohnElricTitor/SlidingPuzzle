using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGimbal : MonoBehaviour
{

    [SerializeField]GameObject gridManager;
    int width;
    int height;
    float offSet;
   
    // Start is called before the first frame update
    void Start()
    {
        width = gridManager.GetComponent<GridManager>().gridX;
        height = gridManager.GetComponent<GridManager>().gridZ;
        offSet = gridManager.GetComponent<GridManager>().gridSpacingOffset;

        transform.position = new Vector3(((width * offSet) - offSet) / 2 , 0, ((height * offSet) - offSet) / 2);
    }
}
