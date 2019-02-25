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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseControl();
    }

    public void pauseControl()
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

    public void gameUIControl()
    {
        if (!isPaused)
            gameCanvas.SetActive(true);
        else
            gameCanvas.SetActive(false);
    }
    public void loadScene( string sceneName )
    {
        if( sceneName == "Main Menu" )
        {
            gameUIControl();
            pauseControl();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
