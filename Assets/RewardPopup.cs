using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RewardPopup : MonoBehaviour
{
    public GameObject panel;      // Reward panel UI
    public Image rewardIcon;      // Icon image
    public TMP_Text rewardText;   // Reward description
    public float duration = 2f;   // How long to show

    private Coroutine hideCoroutine;

    void Start()
    {
        panel.SetActive(false);   // hide at start
    }

    public void ShowReward(Sprite icon, string text)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        rewardIcon.sprite = icon;
        rewardText.text = text;

        panel.SetActive(true);

        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.SetActive(false);
    }
}
