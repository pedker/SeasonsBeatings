using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{


    
    public static GameManager instance { get; private set; }

    
    public int score { get; private set; } = 0;

    [SerializeField] TextMeshProUGUI timerText = null;
    public float timeLeft = 1200f;
    private float timeMin = 0f;
    private float timeSec = 0f;

    [SerializeField] string pointsPlusFileName = null;
    [SerializeField] float pointsPlusVolume = 0.65f;
    [SerializeField] float pointsPlusPitchMinimum = 0.95f;
    [SerializeField] float pointsPlusPitchMaximum = 1.05f;

    AudioPlayer m_audioPlayer;

    void Awake()
    {
        instance = this;
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(pointsPlusFileName);
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        UpdateTimer();
        
    }

    void UpdateTimer()
    {
        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;
        else
            timeLeft = 0;
        timeMin = timeLeft / 60;
        timeSec = timeLeft % 60;
        if (timeSec <= 10)
        {
            timerText.text = string.Format("{0}:0{1}", (int)timeMin, (int)timeSec);
        }
        else
        {
            timerText.text = string.Format("{0}:{1}", (int)timeMin, (int)timeSec);
        }
    }

    public void addScore(int score)
    {
        this.score += score;
        m_audioPlayer.playSFX(pointsPlusFileName, pointsPlusVolume, pointsPlusPitchMinimum, pointsPlusPitchMaximum);
    }
}
