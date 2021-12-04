using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject title = null;
    [SerializeField] Vector3 pausePos = new Vector3 (0,0,0);
    [SerializeField] Vector3 playPos = new Vector3(0, 0, 0);
    [SerializeField] bool isPaused = true;
    private Animator anim = null;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventManager.OnUI += MenuState;
    }

    private void OnDisable()
    {
        EventManager.OnUI -= MenuState;
    }
    private void Update()
    {
        if (isPaused)
            Pause();
        if (!isPaused)
            Play();
    }

    void MenuState()
    {
        isPaused = !isPaused;
        anim.SetBool("isPaused", isPaused);
    }
    void Pause()
    {
        if(title.activeSelf != true)
            title.SetActive(true);
        if(title.transform.position != pausePos)
            title.transform.position = Vector3.MoveTowards(title.transform.position, pausePos, 0.2f);
    }
    
    void Play()
    {
        if(title.transform.position != playPos)
            title.transform.position = Vector3.MoveTowards(title.transform.position, playPos, 0.2f);
        if (title.transform.position == playPos)
            title.SetActive(false);
    }
}

