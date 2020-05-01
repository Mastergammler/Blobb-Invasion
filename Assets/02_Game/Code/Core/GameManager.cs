using UnityEngine;

using BlobbInvasion.Gameplay;

namespace BlobbInvasion.Core
{
    public class GameManager : MonoBehaviour
    {
        //###############
        //##  MEMBERS  ##
        //###############

        private bool mGameRunning = true;
        private Highscore mHighscore;

        //################
        //##    MONO    ##
        //################

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1f;
            mHighscore = new Highscore(false);
            MusicManager.Instance.PlayMusic(false);
            MasterFactory.Instance.SetHighscoreManager(mHighscore);
        }

        //#################
        //##  ACCESSORS  ##
        //#################

        public static GameManager Instance { private set; get; }
        public Highscore Highscore => mHighscore;

        //#################
        //##  INTERFACE  ##
        //#################

        public void PauseResume()
        {
            if (mGameRunning)
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
            mHighscore.PersistScore();
            GetComponent<SceneLoader>().LoadMenu();
        }
    }
}

