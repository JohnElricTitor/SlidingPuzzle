using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    GameObject empty;
    float xTemp;
    float zTemp;

    [SerializeField] float rayLength;


    private void Update()
    {
        RaycastHit hit;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if(Input.GetMouseButtonUp(0))
        {
            GameObject empty = GameObject.Find("Empty(Clone)");
            if (Physics.Raycast(ray, out hit, rayLength))
            {
               if(Vector3.Distance(hit.transform.position, empty.transform.position) == 2)
               {
                    xTemp = hit.transform.position.x;
                    zTemp = hit.transform.position.z;
                    hit.transform.position = new Vector3(empty.transform.position.x, 0, empty.transform.position.z);
                    empty.transform.position = new Vector3(xTemp, 0, zTemp);
               }
            }
        }
    }
}
