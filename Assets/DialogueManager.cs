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

    [Header("Typing Settings")]
    public float typingSpeed = 0.04f;   // time per character
    public float lineDelay = 1.2f;      // delay after line fully typed

    private DialogueLine[] currentDialogueLines;
    private string currentLegendName;
    private Sprite currentLegendPortrait;
    private int lineIndex;

    // Reward for this legend
    private Sprite currentRewardSprite;
    private string currentRewardText;

    private Coroutine typingCoroutine;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    /// <summary>
    /// Start the dialogue sequence for a legend.
    /// </summary>
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

        currentRewardSprite = legendRewardSprite;
        currentRewardText = legendRewardText;

        ShowLine();
    }

    /// <summary>
    /// Show a dialogue line with typewriter effect and auto-next.
    /// </summary>
    void ShowLine()
    {
        StopTypingCoroutine();

        DialogueLine line = currentDialogueLines[lineIndex];

        if (line.portrait != null)
            portraitImage.sprite = line.portrait;

        typingCoroutine = StartCoroutine(TypeLine(line.text));
    }

    IEnumerator TypeLine(string text)
    {
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait a short delay then automatically go to next line
        yield return new WaitForSeconds(lineDelay);
        NextLine();
    }

    void StopTypingCoroutine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
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
            // Dialogue finished, show reward if any
            if (currentRewardSprite != null)
            {
                rewardPopup.ShowReward(currentRewardSprite, currentRewardText);
            }

            StartCoroutine(CloseDialogueDelayed(0.1f));
        }
    }

    public void CloseDialogue()
    {
        StopTypingCoroutine();
        lineIndex = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator CloseDialogueDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseDialogue();
    }
}
