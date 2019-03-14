using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] GameObject UI = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] TextMeshProUGUI timer = null;

    [SerializeField] GameObject pauseMenu = null;

    [SerializeField] GameObject endScreen = null;
    [SerializeField] TextMeshProUGUI message = null;
    [SerializeField] TextMeshProUGUI endScore = null;
    [SerializeField] TextMeshProUGUI endPrice = null;

    bool isPaused = false;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        scoreText.text = "$" + GameManager.instance.score;
        timer.text = "Time Left:\n" + (int) GameManager.instance.countdown;
        if (GameManager.instance.countdown == 0) EndGame(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseControl();
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

        if (isPaused)
            UI.SetActive(false);
        else
            UI.SetActive(true);
    }
  
    public void EndGame(bool win)
    {
        Time.timeScale = 0;
        int score = GameManager.instance.score;
        endScreen.SetActive(true);
        
        if (win)
        {
            message.text = "YOU SURVIVED";
            message.color = Color.green;
        }
        else 
        {
            message.text = "YOU DIED";
            message.color = Color.red;
        }

        endScore.text = "Total Value of Your Cart: $" + score;
        endPrice.text = "You Paid: $" + Mathf.Round(score * .25f * 100f) / 100f;

        Destroy(this);
    }
}
