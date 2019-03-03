using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu; 
    public GameObject gameCanvas;
    bool isPaused = false;
    bool inGame = true;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
            GameUIControl();    
        }
    }

    public void PauseControl()
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

    public void LoadScene(string sceneName)
    {
        if (sceneName == "Main Menu")
        {
            GameUIControl();
            PauseControl();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
