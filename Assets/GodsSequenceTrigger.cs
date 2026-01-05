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
    public float fadeDuration = 1.3f;
    public float fragmentRiseDuration = 1.5f;
    public float displayDuration = 1.8f;
    public float fadeOutDuration = 1.3f;

    private bool triggered = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

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
        /* =========================
         * 1️⃣ Fade in gods
         * ========================= */
        foreach (GameObject god in gods)
        {
            var sr = god.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            god.SetActive(true);
        }

        yield return FadeGods(0f, 0.6f, fadeDuration);

        /* =========================
         * 2️⃣ Rise fragments
         * ========================= */
        foreach (var frag in fragments) frag.SetActive(true);

        Vector3[] startPos = new Vector3[fragments.Length];
        Vector3 targetPos = heartOfSelf.transform.position;

        for (int i = 0; i < fragments.Length; i++)
            startPos[i] = fragments[i].transform.position;

        yield return RiseFragments(startPos, targetPos, fragmentRiseDuration);

        foreach (var frag in fragments) frag.SetActive(false);

        /* =========================
         * 3️⃣ Show heart + text
         * ========================= */
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

        yield return FadeHeartAndText(heartSR, textGroup, baseColor, true, fadeDuration);

        /* =========================
         * 4️⃣ Hold
         * ========================= */
        yield return Hold(displayDuration);

        /* =========================
         * 5️⃣ Fade out
         * ========================= */
        yield return FadeHeartAndText(heartSR, textGroup, baseColor, false, fadeOutDuration);

        heartOfSelf.SetActive(false);
        fragmentCombinedText.gameObject.SetActive(false);
        textGroup.alpha = 1f;
    }

    /* =========================
     * HELPERS (Realtime based)
     * ========================= */

    IEnumerator FadeGods(float from, float to, float duration)
    {
        float start = Time.realtimeSinceStartup;
        float end = start + duration;

        while (Time.realtimeSinceStartup < end)
        {
            float t = Mathf.InverseLerp(start, end, Time.realtimeSinceStartup);
            foreach (var god in gods)
            {
                var sr = god.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(from, to, t));
            }
            yield return null;
        }
    }

    IEnumerator RiseFragments(Vector3[] start, Vector3 target, float duration)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + duration;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.InverseLerp(startTime, endTime, Time.realtimeSinceStartup);
            for (int i = 0; i < fragments.Length; i++)
                fragments[i].transform.position = Vector3.Lerp(start[i], target, t);

            yield return null;
        }

        for (int i = 0; i < fragments.Length; i++)
            fragments[i].transform.position = target;
    }

    IEnumerator FadeHeartAndText(
        SpriteRenderer heart,
        CanvasGroup text,
        Color baseColor,
        bool fadeIn,
        float duration)
    {
        float start = Time.realtimeSinceStartup;
        float end = start + duration;

        while (Time.realtimeSinceStartup < end)
        {
            float t = Mathf.InverseLerp(start, end, Time.realtimeSinceStartup);
            float alpha = fadeIn ? t : 1f - t;

            heart.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            text.alpha = alpha;

            yield return null;
        }
    }

    IEnumerator Hold(float seconds)
    {
        float end = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < end)
            yield return null;
    }
}
