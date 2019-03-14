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
    

    public void addScore(int score)
    {
        this.score += score;
        m_audioPlayer.playSFX(pointsPlusFileName, pointsPlusVolume, pointsPlusPitchMinimum, pointsPlusPitchMaximum);
    }
}
