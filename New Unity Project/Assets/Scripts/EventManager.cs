using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;
    public delegate void ClickAction();
    public static event ClickAction OnClicked;
    public static event ClickAction OnTileMovement;
    public static event ClickAction OnPause;
    public static event ClickAction OnUnPause;
    public static event ClickAction OnNextLvl;

    public static event ClickAction onWinCheck;


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

    public void onPause()
    {
        if (OnPause != null)
            OnPause();
    }

    public void onUnPause()
    {
        if (OnUnPause != null)
            OnUnPause();
    }

    public void NextLvl()
    {
        if (OnNextLvl != null)
            OnNextLvl();
    }


    public void WinCheck()
    {
        if (onWinCheck != null)
            onWinCheck();
    }
}
