using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager fuckshit;
    public delegate void ClickAction();
    public static event ClickAction OnClicked;
    public static event ClickAction OnUI;

    private void Awake()
    {
        fuckshit = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(OnClicked != null)
                OnClicked();
        }
    }

    public void ChangeLevel()
    {
        if (OnUI != null)
            OnUI();
    }

}
