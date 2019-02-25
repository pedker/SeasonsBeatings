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

    public AudioSource m_BGMPlayer;

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
    }

    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }


    // sets the background music to the name of the inputted string in Resources/Audio/Music
    public void setBGM(string clip)
    {
        AudioClip newClip = Resources.Load($"Audio/Music/{clip}") as AudioClip;
        m_BGMPlayer.PlayOneShot(newClip);
    }

    // takes a float value between 0 and 1
    // sets the volume of the background music
    public void setBGMVolume(float value)
    {
        m_BGMPlayer.volume = value;
    }

    // fades the volume to end value
    // takes time amount of time to complete
    public void fadeBGMVolume(float finalVolume, float time)
    {
        StartCoroutine(fadeVolumeCoroutine(finalVolume, time));
    }


    private IEnumerator fadeVolumeCoroutine(float finalVolume, float time)
    {
        float startVolume = m_BGMPlayer.volume;
        while (m_BGMPlayer.volume > 0)
        {
            m_BGMPlayer.volume -= startVolume * Time.deltaTime / time;

            yield return null;
        }
    }
}
