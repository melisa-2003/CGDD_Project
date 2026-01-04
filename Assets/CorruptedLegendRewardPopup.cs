using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CorruptedLegendRewardPopup : MonoBehaviour
{
    public GameObject panel;
    public Image rewardIcon;
    public TMP_Text rewardText;
    public float duration = 2.5f;

    private Coroutine hideCoroutine;

    void Start()
    {
        panel.SetActive(false);
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
