using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameCanvas;
    public GameObject mainMenuCanvas;
    private GameManager gameManager;
    bool isPaused = false;
    bool inGame = false;
    bool atMainMenu = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
            GameUIControl();
        }
    }

    public void MainMenuUIControl()
    {
        atMainMenu = !atMainMenu;
        if (atMainMenu)
            mainMenuCanvas.SetActive(true);
        else
            mainMenuCanvas.SetActive(false);
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
    public void LoadScene( string sceneName )
    {
        if( sceneName == "Main Menu" )
        {
            MainMenuUIControl();
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
