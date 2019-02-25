using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public int score = 0;
    public Text scoreText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        SceneManager.sceneLoaded += OnSceneLoaded; //Use this in vain of Start()
    }
    void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneMode)
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
