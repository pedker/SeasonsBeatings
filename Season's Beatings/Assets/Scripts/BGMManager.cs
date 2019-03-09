using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance { get; private set; }
    AudioSource m_BGMPlayer;

    private void Awake()
    {
        instance = this;
        m_BGMPlayer = GetComponent<AudioSource>();
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
