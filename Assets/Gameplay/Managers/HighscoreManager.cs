using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour
{

    public const int KILL_ENEMY = 100;
    public const int FIND_CORE = 1000;
    public const int FIND_BULLET = 500;
    public const int GET_HEALTH = 50;


    public static HighscoreManager Instance { private set;get;}

    private static int HighestScoreEver = 0;
    private static int Highscore = 0;
    private TextMeshProUGUI mText;

    private void Awake() {
        if(Instance != null) Destroy(gameObject);
        Instance = this;
        SceneManager.sceneLoaded += sceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    private void sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            GameObject obj = GameObject.Find("HighScoreValue");
            mText = obj.GetComponent<TextMeshProUGUI>();
            if(mText != null)
            {
                mText.text = Highscore.ToString();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    public void AddToScore(int value)
    {
        Highscore += value;
    }

    public void NewGame()
    {
        if(HighestScoreEver < Highscore)
        {
            HighestScoreEver = Highscore;
        }
        Highscore = 0;
    }


}
