using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0,1), Time.deltaTime * rotateSpeed);
    }
}
