using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CorruptedLegendDialogueManager : MonoBehaviour
{
    public static CorruptedLegendDialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public Button closeButton;

    [Header("Reward Settings")]
    public CorruptedLegendRewardPopup rewardPopup;

    [Header("Typing Settings")]
    public float typingSpeed = 0.1f;
    public float lineDelay = 1.2f;

    private CorruptedLegendDialogueLine[] currentDialogueLines;
    private string currentLegendName;
    private Sprite currentLegendPortrait;
    private int lineIndex;

    private Sprite currentRewardSprite;
    private string currentRewardText;

    private Coroutine typingCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    public void StartDialogue(
        string legendName,
        Sprite legendPortrait,
        CorruptedLegendDialogueLine[] lines,
        Sprite legendRewardSprite,
        string legendRewardText
    )
    {
        currentLegendName = legendName;
        currentLegendPortrait = legendPortrait;
        currentDialogueLines = lines;
        lineIndex = 0;

        currentRewardSprite = legendRewardSprite;
        currentRewardText = legendRewardText;

        dialoguePanel.SetActive(true);
        nameText.text = currentLegendName;
        portraitImage.sprite = currentLegendPortrait;

        ShowLine();
    }

    void ShowLine()
    {
        StopTypingCoroutine();

        CorruptedLegendDialogueLine line = currentDialogueLines[lineIndex];
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
            // Show reward
            if (currentRewardSprite != null && rewardPopup != null)
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
