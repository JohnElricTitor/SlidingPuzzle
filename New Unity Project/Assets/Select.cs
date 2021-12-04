using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public GameObject[] select = new GameObject[4];
    Ray rayUp, rayDown, rayLeft, rayRight;                              //4 DIRECTIONAL ARRAYS
    bool isUp, isDown, isLeft, isRight;                                 //BOOLEANS THAT ARE USED TO STORE WHAT DIRECTIONAL RAYCASTS HITS A PATH TILE 
    [SerializeField] float rayOffSet = 0;
    [SerializeField] float rayLength = 0;


    private void Update()
    {
        DetectTile();
    }
    public void DetectTile()
    {
        rayUp = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + rayOffSet), Vector3.forward);    //RAYCAST FOR UP DIRECTION                      
        rayDown = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - rayOffSet), Vector3.back);     //RAYCAST FOR DOWN DIRECTION
        rayLeft = new Ray(new Vector3(transform.position.x - rayOffSet, transform.position.y, transform.position.z), Vector3.left);     //RAYCAST FOR LEFT DIRECTION
        rayRight = new Ray(new Vector3(transform.position.x + rayOffSet, transform.position.y, transform.position.z), Vector3.right);   //RAYCAST FOR RIGHT DIRECTION


    }
}
