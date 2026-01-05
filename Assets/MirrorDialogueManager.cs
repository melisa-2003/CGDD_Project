using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class MirrorDialogueManager : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public float lineDuration = 2.5f;

    public float characterDelay;


    public void StartDialogue(MirrorDialogueData data, Action onFinish)
    {
        dialoguePanel.SetActive(true);
        nameText.text = data.speakerName;

        StartCoroutine(PlayDialogue(data, onFinish));
    }

    private IEnumerator PlayDialogue(MirrorDialogueData data, Action onFinish)
    {
        foreach (string line in data.lines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(lineDuration);
        }

        dialoguePanel.SetActive(false);
        onFinish?.Invoke();
    }

    
}
