using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RewardPopup : MonoBehaviour
{
    public GameObject panel;
    public Image rewardIcon;
    public TMP_Text rewardText;

    public float duration = 2f;

    private Coroutine hideCoroutine;

    void Start()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// Show reward popup with icon and text
    /// </summary>
    public void ShowReward(Sprite icon, string text)
    {
        // Stop previous hide coroutine if any
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // Update content
        rewardIcon.sprite = icon;
        rewardText.text = text;

        // Show panel
        panel.SetActive(true);

        // Auto-hide after duration
        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.SetActive(false);
    }
}
