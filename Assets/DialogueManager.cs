using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Buttons")]
    public Button choiceButton;     // Yes
    public Button closeButton;      // Close

    [Header("Dialogue Data")]
    public Sprite hangTuahPortrait;
    public RewardPopup rewardPopup;
    public Sprite courageFragmentSprite;

    private string[] hangTuahLines =
    {
        "Stranger, this light… belongs to me.",
        "Courage is not just fighting — it is choosing to step forward.",
        "Will you keep going?"
    };

    private int index = 0;
    

    void Start()
    {
        dialoguePanel.SetActive(false);
        choiceButton.gameObject.SetActive(false);
        closeButton.onClick.AddListener(CloseDialogue);

        choiceButton.onClick.AddListener(PlayerSaysYes);
    }

    public void StartHangTuahDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        nameText.text = "Hang Tuah (Pure Form)";
        portraitImage.sprite = hangTuahPortrait;

        StartCoroutine(AutoRunDialogue());
    }

    // Auto-run dialogue line by line
    private IEnumerator AutoRunDialogue()
    {
        while (index < hangTuahLines.Length)
        {
            dialogueText.text = hangTuahLines[index];

            // If this is the choice line, pause and wait for player input
            if (index == hangTuahLines.Length - 1)
            {
                choiceButton.gameObject.SetActive(true);
                
                yield break;  // stop coroutine until player chooses Yes
            }

            index++;
            yield return new WaitForSeconds(3f); // wait 3 seconds between lines
        }

        // After last line auto-complete
        CloseDialogue();
    }

    // Called when player presses Yes
    private void PlayerSaysYes()
    {
        choiceButton.gameObject.SetActive(false);

        // Show final message
        dialogueText.text =
            "When fear comes, and you still move forward… That is courage.\n" +
            "I grant you the Fragment of Courage. Go on — more truths await you.";

        // Show reward
        rewardPopup.ShowReward(courageFragmentSprite, "Courage Fragment +1");

        // Optional: auto-close panel after a delay
        StartCoroutine(AutoCloseAfterSeconds(4f));
    }

    private IEnumerator AutoCloseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CloseDialogue();
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        StopAllCoroutines();
    }
}
