using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChannel : MonoBehaviour
{
    private AudioSource m_audioPlayer;
    public bool inUse = false;

    void Awake()
    {
        m_audioPlayer = this.GetComponent<AudioSource>();
        inUse = false;
    }

    // This function should only be called by AudioPlayer.cs
    public void playSound(AudioClip sound, float soundVolume, float minimumPitch, float maximumPitch)
    {
        StartCoroutine( playSoundCoroutine(sound, soundVolume, minimumPitch, maximumPitch) );
    }


    private IEnumerator playSoundCoroutine(AudioClip sound, float soundVolume, float minimumPitch, float maximumPitch)
    {
        inUse = true;

        float timePassed = 0.0f;
        m_audioPlayer.pitch = Random.Range(minimumPitch, maximumPitch);
        m_audioPlayer.PlayOneShot(sound, soundVolume);

        while (timePassed < sound.length)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        m_audioPlayer.pitch = 1.0f;

        inUse = false;
    }
}
