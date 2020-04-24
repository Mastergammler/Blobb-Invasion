using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance { private set; get; }

    private bool mGameRunning = true;

    private void Awake() 
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        MusicManager.Instance.PlayMusic(false);
    }

    public void PauseResume()
    {
        if(mGameRunning)
        {
            mGameRunning = false;
            Time.timeScale = 0f;
        }
        else
        {
            mGameRunning = true;
            Time.timeScale = 1;
        }
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        GetComponent<SceneLoader>().LoadMenu();
    }
}
