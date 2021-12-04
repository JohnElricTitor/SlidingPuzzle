using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TileType : MonoBehaviour
{
    public enum tileType { path,start,end, empty };
    public tileType type;


    [HideInInspector] public enum TileDirection {top,bottom,left,right};
    [HideInInspector] public TileDirection direction;
}
