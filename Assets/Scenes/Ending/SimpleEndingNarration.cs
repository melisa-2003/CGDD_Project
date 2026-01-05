using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleEndingNarration : MonoBehaviour
{
    public TMP_Text narrationText;

    [TextArea(10, 20)]
    public string[] narrationLines;

    [Header("Timing")]
    public float typingSpeed = 0.045f;
    public float linePause = 1.6f;
    public float endPause = 2.5f;

    [Header("UI")]
    public Button skipButton;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private bool skipRequested = false;

    void Start()
    {
        narrationText.text = "";

        if (skipButton != null)
            skipButton.onClick.AddListener(SkipNarration);

        StartCoroutine(PlayNarration());
    }

    void SkipNarration()
    {
        skipRequested = true;
    }

    IEnumerator PlayNarration()
    {
        foreach (string line in narrationLines)
        {
            narrationText.text = "";
            yield return StartCoroutine(TypeLine(line));

            if (skipRequested) break;

            yield return new WaitForSeconds(linePause);
        }

        // 🌟 Final Thank You line
        narrationText.text = "";
        yield return StartCoroutine(TypeLine("Thank you for playing"));
        yield return new WaitForSeconds(endPause);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            if (skipRequested)
            {
                narrationText.text = line; // instantly show full line
                yield break;
            }

            narrationText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
