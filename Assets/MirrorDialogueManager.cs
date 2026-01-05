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
    public float lineDuration = 2.5f;
    public float characterDelay; // (unused for now, kept for future typing effect)

    private Coroutine dialogueCoroutine;
    private Action onFinishCallback;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    public void StartDialogue(MirrorDialogueData data, Action onFinish)
    {
        // Safety: stop previous dialogue if any
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialoguePanel.SetActive(true);
        nameText.text = data.speakerName;

        onFinishCallback = onFinish;
        dialogueCoroutine = StartCoroutine(PlayDialogue(data));
    }

    private IEnumerator PlayDialogue(MirrorDialogueData data)
    {
        foreach (string line in data.lines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        CloseDialogue();
    }

    /// <summary>
    /// Called by Close Button
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
