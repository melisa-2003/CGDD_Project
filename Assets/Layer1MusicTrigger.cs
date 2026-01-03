using UnityEngine;
using System.Collections;

public class Layer1MusicTrigger : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusic;
    public AudioSource birdAmbience;

    [Header("Volume Settings")]
    public float musicTargetVolume = 0.6f;
    public float birdTargetVolume = 0.2f;
    public float fadeSpeed = 0.15f;

    void Start()
    {
        if (backgroundMusic != null && birdAmbience != null)
        {
            StartCoroutine(FadeInAudio());
        }
        else
        {
            Debug.LogError("AudioSource missing on Layer1MusicTrigger!");
        }
    }

    IEnumerator FadeInAudio()
    {
        backgroundMusic.volume = 0f;
        birdAmbience.volume = 0f;

        backgroundMusic.Play();
        birdAmbience.Play();

        while (backgroundMusic.volume < musicTargetVolume ||
               birdAmbience.volume < birdTargetVolume)
        {
            if (backgroundMusic.volume < musicTargetVolume)
                backgroundMusic.volume += Time.deltaTime * fadeSpeed;

            if (birdAmbience.volume < birdTargetVolume)
                birdAmbience.volume += Time.deltaTime * fadeSpeed;

            yield return null;
        }
    }
}
