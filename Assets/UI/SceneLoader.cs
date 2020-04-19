using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadGame()
    {
        HighscoreManager.Instance.NewGame();
        SceneManager.LoadScene(GAME_SCENE,LoadSceneMode.Single);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE,LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
