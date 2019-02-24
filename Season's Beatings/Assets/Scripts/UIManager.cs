using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    GameObject[] pausedObjects;
    // Start is called before the first frame update
    void Start()
    {
        pausedObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseControl();
        }
    }

    public void pauseControl()
    {
        if( Time.timeScale == 1 )
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if ( Time.timeScale == 0 )
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }
    public void showPaused()
    {
        foreach(GameObject g in pausedObjects)
            g.SetActive(true);
    }
    public void hidePaused()
    {
        foreach(GameObject g in pausedObjects)
            g.SetActive(false);
    }
    public void loadScene( string sceneName )
    {
        SceneManager.LoadScene(sceneName);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
