using UnityEngine;
using System.Collections;

public class BGMFadeIn : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1.5f;
    public float targetVolume = 1f;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}

