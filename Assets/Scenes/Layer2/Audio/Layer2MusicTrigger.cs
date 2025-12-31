using UnityEngine;
using System.Collections;

public class Layer2MusicTrigger : MonoBehaviour
{
    AudioSource layer2Music;

    void Awake()
    {
        layer2Music = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (layer2Music != null)
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
        layer2Music.volume = 0f;
        layer2Music.Play();

        while (layer2Music.volume < 0.6f)
        {
            layer2Music.volume += Time.deltaTime * 0.15f;
            yield return null;
        }
    }
}

//layer2才会play music