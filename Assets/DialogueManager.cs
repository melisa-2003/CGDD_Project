using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public Button closeButton;

    [Header("Reward Settings")]
    public RewardPopup rewardPopup;

    private DialogueLine[] currentDialogueLines;
    private string currentLegendName;
    private Sprite currentLegendPortrait;
    private int lineIndex;

    // Reward for this legend
    private Sprite currentRewardSprite;
    private string currentRewardText;

    void Start()
    {
        dialoguePanel.SetActive(false);
        closeButton.onClick.AddListener(CloseDialogue);
    }

    // Start dialogue for a specific legend
    public void StartDialogue(
        string legendName,
        Sprite legendPortrait,
        DialogueLine[] lines,
        Sprite legendRewardSprite,
        string legendRewardText
    )
    {
        currentLegendName = legendName;
        currentLegendPortrait = legendPortrait;
        currentDialogueLines = lines;
        lineIndex = 0;

        dialoguePanel.SetActive(true);
        nameText.text = currentLegendName;
        portraitImage.sprite = currentLegendPortrait;

        // Assign this legend's reward
        currentRewardSprite = legendRewardSprite;
        currentRewardText = legendRewardText;

        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines(); // 🔥 VERY IMPORTANT

        DialogueLine line = currentDialogueLines[lineIndex];
        dialogueText.text = line.text;

        if (line.portrait != null)
            portraitImage.sprite = line.portrait;

        StartCoroutine(AutoNextLine(2f));
    }


    IEnumerator AutoNextLine(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextLine();
    }

    public void NextLine()
{
    lineIndex++;

    if (lineIndex < currentDialogueLines.Length)
    {
        ShowLine();
    }
    else
    {
        if (currentRewardSprite != null)
        {
            rewardPopup.ShowReward(currentRewardSprite, currentRewardText);
        }

        StartCoroutine(CloseDialogueDelayed(0.1f));
    }
}

    public void CloseDialogue()
    {
        StopAllCoroutines();
        lineIndex = 0;
        dialoguePanel.SetActive(false);
    }

IEnumerator CloseDialogueDelayed(float delay)
{
    yield return new WaitForSeconds(delay);
    CloseDialogue();
}


}
