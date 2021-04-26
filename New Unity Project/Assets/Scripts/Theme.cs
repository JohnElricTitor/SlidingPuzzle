using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCRIPTABLE OBJECT THAT IS USED TO STORE EACH THEMES PATH TILE AND ALL THE ENVIRONMENT TILES

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public GameObject[] Environment;
    public GameObject startTile;
    public GameObject endTile;
    public GameObject pathTile;
}
