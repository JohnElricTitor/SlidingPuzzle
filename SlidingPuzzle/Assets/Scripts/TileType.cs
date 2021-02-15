using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TileType : MonoBehaviour
{
    public enum tileType {path, end, empty};
    public tileType type;

    
}
