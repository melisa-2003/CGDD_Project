using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class MirrorDialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Button closeButton;

    [Header("Timing")]
    public float lineDuration = 1.5f;   // Time to wait after typing each line
    public float characterDelay = 0.03f; // Typing speed per character

    private Coroutine dialogueCoroutine;
    private Action onFinishCallback;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    /// <summary>
    /// Starts a dialogue sequence
    /// </summary>
    public void StartDialogue(MirrorDialogueData data, Action onFinish)
    {
        // Stop previous dialogue if any
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialoguePanel.SetActive(true);
        nameText.text = data.speakerName;

        onFinishCallback = onFinish;
        dialogueCoroutine = StartCoroutine(PlayDialogue(data));
    }

    /// <summary>
    /// Plays dialogue lines with typing effect
    /// </summary>
    private IEnumerator PlayDialogue(MirrorDialogueData data)
    {
        foreach (string line in data.lines)
        {
            yield return StartCoroutine(TypeLine(line));
            // Wait after line is fully typed
            yield return new WaitForSeconds(lineDuration);
        }

        CloseDialogue();
    }

    /// <summary>
    /// Types out a single line character by character
    /// </summary>
    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(characterDelay);
        }
    }

    /// <summary>
    /// Closes the dialogue panel
    /// </summary>
    public void CloseDialogue()
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        dialoguePanel.SetActive(false);

        onFinishCallback?.Invoke();
        onFinishCallback = null;
    }
}
