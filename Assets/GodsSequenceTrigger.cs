using UnityEngine;
using TMPro;
using System.Collections;

public class GodsSequenceTrigger : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject[] gods;
    public GameObject[] fragments;
    public GameObject heartOfSelf;
    public TMP_Text fragmentCombinedText;

    [Header("Timing")]
    public float fadeDuration = 1.5f;
    public float fragmentRiseDuration = 2.5f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1.5f;

    [Header("Curve Settings")]
    public float curveHeight = 2f;

    private bool triggered = false;

    void Start()
    {
        foreach (var god in gods) god.SetActive(false);
        foreach (var frag in fragments) frag.SetActive(false);
        heartOfSelf.SetActive(false);
        fragmentCombinedText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Fade in gods
        foreach (var god in gods)
        {
            var sr = god.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            god.SetActive(true);
        }
        yield return FadeGods(0f, 0.6f, fadeDuration);

        // Fragments rise
        foreach (var frag in fragments) frag.SetActive(true);

        Vector3[] startPos = new Vector3[fragments.Length];
        for (int i = 0; i < fragments.Length; i++)
            startPos[i] = fragments[i].transform.position;

        yield return RiseFragmentsLocal(startPos, heartOfSelf.transform.position, fragmentRiseDuration, curveHeight);

        foreach (var frag in fragments) frag.SetActive(false);

        // Heart + text appear with scale
        heartOfSelf.transform.localScale = Vector3.zero;
        heartOfSelf.SetActive(true);
        var heartSR = heartOfSelf.GetComponent<SpriteRenderer>();
        Color baseColor = heartSR.color;
        heartSR.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);

        CanvasGroup textGroup = fragmentCombinedText.GetComponent<CanvasGroup>();
        if (!textGroup)
            textGroup = fragmentCombinedText.gameObject.AddComponent<CanvasGroup>();

        textGroup.alpha = 0f;
        fragmentCombinedText.gameObject.SetActive(true);
        fragmentCombinedText.ForceMeshUpdate();

        // Animate heart fade + scale
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            heartSR.color = new Color(baseColor.r, baseColor.g, baseColor.b, t);
            heartOfSelf.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            textGroup.alpha = t;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        heartSR.color = baseColor;
        heartOfSelf.transform.localScale = Vector3.one;
        textGroup.alpha = 1f;

        // Hold
        yield return Hold(displayDuration);

        // Fade out
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            float t = Mathf.Clamp01(elapsed / fadeOutDuration);
            heartSR.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f - t);
            textGroup.alpha = 1f - t;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        heartOfSelf.SetActive(false);
        fragmentCombinedText.gameObject.SetActive(false);
        textGroup.alpha = 1f;
    }

    // =========================
    // Fragments rise toward heart using **local offsets**
    // =========================
    IEnumerator RiseFragmentsLocal(Vector3[] start, Vector3 target, float duration, float height)
    {
        float elapsed = 0f;

        // Compute small offsets for curve
        Vector3[] apex = new Vector3[fragments.Length];
        for (int i = 0; i < fragments.Length; i++)
        {
            // Apex is midpoint + upward offset proportional to screen
            apex[i] = Vector3.Lerp(start[i], target, 0.5f) + Vector3.up * height;
        }

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);

            for (int i = 0; i < fragments.Length; i++)
            {
                // Quadratic Bezier
                Vector3 pos = Mathf.Pow(1 - t, 2) * start[i] + 2 * (1 - t) * t * apex[i] + Mathf.Pow(t, 2) * target;
                fragments[i].transform.position = pos;
            }

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure fragments reach target
        for (int i = 0; i < fragments.Length; i++)
            fragments[i].transform.position = target;
    }

    IEnumerator FadeGods(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            foreach (var god in gods)
            {
                var sr = god.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(from, to, t));
            }
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        foreach (var god in gods)
        {
            var sr = god.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, to);
        }
    }

    IEnumerator Hold(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
