using UnityEngine;


//THIS SCRIPT IS USED TO ALLOW CINEMACHINE TO CHOOSE WHAT CAMERA IS THE CURRENT PAUSE CAMERA 
[System.Serializable]
public class GameCameras
{
    public GameObject GameCam;
    public GameObject PauseCamBL;
    public GameObject PauseCamBR;
    public GameObject PauseCamTR;
    public GameObject PauseCamTL;
}
public class CamTransitions : MonoBehaviour
{
    [SerializeField] GameCameras cams;
    [SerializeField] GameObject uiMenu;
    int currentPauseCam;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.OnPause += Pause;
        EventManager.OnUnPause += Play;
    }

    private void OnDisable()
    {
        EventManager.OnPause -= Pause;
        EventManager.OnUnPause += Play;
    }

    void PauseCamCheck()
    {
        float bl = Vector3.Distance(cams.GameCam.transform.position, cams.PauseCamBL.transform.position); 
        float br = Vector3.Distance(cams.GameCam.transform.position, cams.PauseCamBR.transform.position);
        float tr = Vector3.Distance(cams.GameCam.transform.position, cams.PauseCamTR.transform.position);
        float tl = Vector3.Distance(cams.GameCam.transform.position, cams.PauseCamTL.transform.position);

        
        if (bl < br && bl < tr && bl < tl)          //BL is closest camera
        {
            uiMenu.transform.rotation = Quaternion.Euler(0, 0, 0);
            currentPauseCam = 0;
        }
        
        else if (br < bl && br < tr && br < tl)     //BR is closest camera
        {
            uiMenu.transform.rotation = Quaternion.Euler(0, -90, 0);
            currentPauseCam = 1;
        }
        
        else if (tr < bl && tr < br && tr < tl)     //TR is closest camera
        {
            uiMenu.transform.rotation = Quaternion.Euler(0,-180,0);
            currentPauseCam = 2;
        }
        
        else if (tl < bl && tl < br && tl < tr)     //TL is closest camera 
        {
            uiMenu.transform.rotation = Quaternion.Euler(0, 90, 0);
            currentPauseCam = 3;
        }
    }

    public void Pause()
    {
        PauseCamCheck();
        anim.SetInteger("currentPauseCam", currentPauseCam);
        anim.SetBool("isPaused", true);
        uiMenu.SetActive(true);
    }

    public void Play()
    {
        anim.SetBool("isPaused", false);
        uiMenu.SetActive(false);
    }
}
