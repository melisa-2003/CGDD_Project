using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionUI : MonoBehaviour
{
    [Header("Fade")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource bgmSource;

    public void LoadPurity()
    {
        StartCoroutine(Transition("purity"));
    }

    IEnumerator Transition(string sceneName)
    {
        float time = 0f;

        fadeCanvas.gameObject.SetActive(true);
        fadeCanvas.alpha = 0f;

        float startVolume = bgmSource != null ? bgmSource.volume : 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;

            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t);

            if (bgmSource != null)
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeCanvas.alpha = 1f;

        SceneManager.LoadScene(sceneName);
    }
}
