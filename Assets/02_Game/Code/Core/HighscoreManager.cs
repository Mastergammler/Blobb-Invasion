using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace BlobbInvasion.Core
{
    public class HighscoreManager : MonoBehaviour
    {

        public const int KILL_ENEMY = 100;
        public const int FIND_CORE = 1000;
        public const int FIND_BULLET = 500;
        public const int GET_HEALTH = 50;

        private const String HIGHSCORE_KEY = "prefs-highscore-key";


        public static HighscoreManager Instance { private set; get; }

        private static int HighestScoreEver = 0;
        private static int Highscore = 0;
        private TextMeshProUGUI mText;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
            SceneManager.sceneLoaded += sceneLoaded;
            DontDestroyOnLoad(gameObject);
        }

        private void sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0)
            {
                if (HighestScoreEver < Highscore)
                {
                    HighestScoreEver = Highscore;
                    PlayerPrefs.SetInt(HIGHSCORE_KEY,HighestScoreEver);
                }
                GameObject obj = GameObject.Find("HighScoreValue");
                mText = obj.GetComponent<TextMeshProUGUI>();

                if (mText != null)
                {
                    mText.text = HighestScoreEver.ToString();
                }
            }
            else if (scene.buildIndex == 1)
            {
                GameObject obj = GameObject.Find("HighScoreValue");
                mText = obj.GetComponent<TextMeshProUGUI>();

                if (mText != null)
                {
                    mText.text = "0";
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            HighestScoreEver = PlayerPrefs.GetInt(HIGHSCORE_KEY,0);
        }

        public void AddToScore(int value)
        {
            Highscore += value;
            mText.text = Highscore.ToString();
        }

        public void NewGame()
        {
            Highscore = 0;
        }


    }
}