using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;
    public delegate void ClickAction();
    public static event ClickAction OnClicked;
    public static event ClickAction OnTileMovement;
    public static event ClickAction OnUI;

    private void Awake()
    {
        eventManager = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(OnClicked != null)
                OnClicked();
        }
    } 

    public void TileMovement()
    {
        if(OnTileMovement != null)
        {            
            OnTileMovement();
        }
    }   

    public void ChangeLevel()
    {
        if (OnUI != null)
            OnUI();
    }

}
