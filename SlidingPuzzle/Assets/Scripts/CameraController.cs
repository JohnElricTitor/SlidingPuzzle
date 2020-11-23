using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 prevPos;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            prevPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 direction = prevPos - Camera.main.ScreenToViewportPoint(Input.mousePosition);

            transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            prevPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
