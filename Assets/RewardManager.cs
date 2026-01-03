using UnityEngine;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    public GameObject panel;
    public TMP_Text rewardText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowReward()
    {
        panel.SetActive(true);
        rewardText.text = "Reward Obtained!";
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
