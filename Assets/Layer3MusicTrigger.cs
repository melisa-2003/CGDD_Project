using UnityEngine;
using System.Collections;

public class Layer3MusicTrigger : MonoBehaviour
{
    AudioSource layer3Music;

    void Awake()
    {
        layer3Music = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (layer3Music != null)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("No AudioSource found on Layer2_Audio!");
        }
    }

    IEnumerator FadeIn()
    {
        layer3Music.volume = 0f;
        layer3Music.Play();

        while (layer3Music.volume < 0.6f)
        {
            layer3Music.volume += Time.deltaTime * 0.15f;
            yield return null;
        }
    }
}
