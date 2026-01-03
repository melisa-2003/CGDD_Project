using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LegendDialogueManager : MonoBehaviour
{
    public static LegendDialogueManager Instance;

    public GameObject panel;
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowDialogue(string name, Sprite portrait)
    {
        panel.SetActive(true);
        nameText.text = name;
        portraitImage.sprite = portrait;
        dialogueText.text = "You have broken the curse...";
    }

    public void Close()
    {
        panel.SetActive(false);
        RewardManager.Instance.ShowReward();
    }
}
