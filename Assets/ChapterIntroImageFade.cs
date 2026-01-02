using UnityEngine;
using System.Collections;

public class ChapterIntroImageFade : MonoBehaviour
{
    public CanvasGroup fadeImageGroup;
    public CanvasGroup chapterTextGroup;

    public float fadeInDuration = 1.5f;
    public float textFadeDuration = 0.8f;
    public float visibleTime = 1.5f;
    public float fadeOutDuration = 1.5f;

    void Awake()
    {
        // Ensure image is visible BEFORE first frame
        fadeImageGroup.alpha = 1f;
        chapterTextGroup.alpha = 0f;
    }

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Optional: slight pause to ensure render
        yield return null;

        // Fade IN text
        yield return Fade(chapterTextGroup, 0f, 1f, textFadeDuration);

        // Stay visible
        yield return new WaitForSeconds(visibleTime);

        // Fade OUT text
        yield return Fade(chapterTextGroup, 1f, 0f, fadeOutDuration);

        // Fade OUT image
        yield return Fade(fadeImageGroup, 1f, 0f, fadeOutDuration);

        // Disable canvas
        gameObject.SetActive(false);
    }

    IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
    {
        float time = 0f;
        group.alpha = from;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        group.alpha = to;
    }
}
