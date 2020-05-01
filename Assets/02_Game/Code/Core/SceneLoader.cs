using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlobbInvasion.Core
{
    public class SceneLoader : MonoBehaviour
    {
        //#################
        //##  CONSTANTS  ##
        //#################
        public const String MAIN_MENU_SCENE = "MainMenu";
        public const String GAME_SCENE = "MainGame";

        //################
        //##    MONO    ##
        //################

        //todo workaround, until i have a real menu manager
        private void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
                new Highscore(true);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}