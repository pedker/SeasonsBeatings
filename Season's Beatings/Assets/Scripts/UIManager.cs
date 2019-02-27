using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu; 
    public GameObject gameCanvas;
    public GameObject mainMenuCanvas; // Useless
    private GameManager gameManager; //Not used. Useless
    bool isPaused = false;
    bool inGame = false;
    bool atMainMenu = true; // Useless
    
    void Start()
    {
        gameManager = GameManager.instance;  //Not used. Useless
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
            GameUIControl();    // Only needed if disabling the UI is really necessary
        }
    }

    public void MainMenuUIControl() // Useless function. Just disable the ui in the main menu scene.
    {
        atMainMenu = !atMainMenu;
        if (atMainMenu)
            mainMenuCanvas.SetActive(true);
        else
            mainMenuCanvas.SetActive(false);
    }

    public void PauseControl() // Now this should be disabled in the main menu scene
    {
        isPaused = !isPaused;
        if( isPaused )
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void GameUIControl()
    {
        inGame = !inGame;
        if (inGame)
            gameCanvas.SetActive(true);
        else
            gameCanvas.SetActive(false);
    }

    public void LoadScene( string sceneName ) // Useless function. Just disable the ui in the main menu scene.
    {
        //if( sceneName == "Main Menu" )    
        {
           // MainMenuUIControl();
            //GameUIControl();
           // PauseControl();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
