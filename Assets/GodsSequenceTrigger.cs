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
    public float fadeDuration = 1f;     
    public float fragmentRiseDuration = 1.5f;
    public float displayDuration = 1.5f; // how long Heart + text stay visible
    public float fadeOutDuration = 1f;   // fade out Heart + text

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator PlaySequence()
    {
        // 1️⃣ Fade in gods
        foreach (GameObject god in gods)
        {
            SpriteRenderer sr = god.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
            god.SetActive(true);
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            foreach (GameObject god in gods)
            {
                SpriteRenderer sr = god.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = Mathf.Lerp(0f, 0.6f, elapsed / fadeDuration); // semi-transparent
                sr.color = c;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2️⃣ Activate and rise fragments
        foreach (GameObject frag in fragments) frag.SetActive(true);

        Vector3[] startPositions = new Vector3[fragments.Length];
        Vector3 targetPos = heartOfSelf.transform.position;

        for (int i = 0; i < fragments.Length; i++)
            startPositions[i] = fragments[i].transform.position;

        elapsed = 0f;
        while (elapsed < fragmentRiseDuration)
        {
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i].transform.position = Vector3.Lerp(
                    startPositions[i],
                    targetPos,
                    elapsed / fragmentRiseDuration
                );
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3️⃣ Snap fragments into Heart
        for (int i = 0; i < fragments.Length; i++)
            fragments[i].transform.position = targetPos;

        foreach (GameObject frag in fragments)
            frag.SetActive(false);

        // 4️⃣ Show Heart + Fragment Combined Text together
        heartOfSelf.SetActive(true);
        if (fragmentCombinedText != null)
        {
            fragmentCombinedText.gameObject.SetActive(true);
        }

        // 5️⃣ Hold for displayDuration
        yield return new WaitForSeconds(displayDuration);

        // 6️⃣ Fade out Heart + text
        float fadeElapsed = 0f;
        SpriteRenderer heartSR = heartOfSelf.GetComponent<SpriteRenderer>();
        Color heartColor = heartSR.color;
        Color textColor = fragmentCombinedText.color;

        while (fadeElapsed < fadeOutDuration)
        {
            float t = fadeElapsed / fadeOutDuration;
            if (heartSR != null)
                heartSR.color = new Color(heartColor.r, heartColor.g, heartColor.b, Mathf.Lerp(1f, 0f, t));
            if (fragmentCombinedText != null)
                fragmentCombinedText.color = new Color(textColor.r, textColor.g, textColor.b, Mathf.Lerp(1f, 0f, t));

            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        // 7️⃣ Make sure fully invisible
        heartOfSelf.SetActive(false);
        if (fragmentCombinedText != null)
        {
            fragmentCombinedText.gameObject.SetActive(false);
            fragmentCombinedText.color = textColor; // reset alpha for next time
        }
    }
}
