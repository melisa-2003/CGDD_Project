using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalBossController : MonoBehaviour
{
    [Header("Boss Components")]
    public Animator bossAnimator;
    public SpriteRenderer bossSprite;

    [Header("Child")]
    public GameObject childObject;

    [Header("Dialogue")]
    public MirrorDialogueManager dialogueManager;
    public MirrorDialogueData shadowDialogue;
    public MirrorDialogueData childDialogue;

    [Header("Dialogue Speed")]
    public float normalDialogueSpeed = 0.04f;
    public float childDialogueSpeed = 0.1f;


    [Header("UI")]
    public Button hugButton;
    public TMP_Text finalMessageText;

    [Header("Boss Settings")]
    public int hitsToDefeat = 20;
    public float hitFlashDuration = 0.1f;

    private int hitCount = 0;
    private bool defeated = false;
    private bool battleStarted = false;

    void Start()
    {
        childObject.SetActive(false);
        hugButton.gameObject.SetActive(false);
        finalMessageText.gameObject.SetActive(false);
    }

    // 🔥 Called by FinalBossTrigger
    public void StartFinalBossSequence()
    {
        if (battleStarted) return;
        battleStarted = true;

        dialogueManager.characterDelay = normalDialogueSpeed;
        dialogueManager.StartDialogue(shadowDialogue, StartAttackPhase);
    }

    void StartAttackPhase()
    {
        bossAnimator.SetBool("isAttacking", true);
    }

    // 🔴 Called by PlayerAttack
    public void TakeHit()
    {
        if (defeated) return;

        hitCount++;
        StartCoroutine(FlashRed());

        if (hitCount >= hitsToDefeat)
        {
            StartCoroutine(DefeatBoss());
        }
    }

    IEnumerator FlashRed()
    {
        Color original = bossSprite.color;
        bossSprite.color = Color.red;
        yield return new WaitForSeconds(hitFlashDuration);
        bossSprite.color = original;
    }

    IEnumerator DefeatBoss()
    {
        defeated = true;

        bossAnimator.SetBool("isAttacking", false);
        bossAnimator.SetTrigger("ToChild");

        yield return new WaitForSeconds(1.5f);

        bossSprite.enabled = false;
        childObject.SetActive(true);

        dialogueManager.characterDelay = childDialogueSpeed;
        dialogueManager.StartDialogue(childDialogue, EnableHug);
    }

    void EnableHug()
    {
        hugButton.gameObject.SetActive(true);
        hugButton.onClick.RemoveAllListeners();
        hugButton.onClick.AddListener(() =>
        {
            hugButton.gameObject.SetActive(false);
            StartCoroutine(HugSequence());
        });
    }

    IEnumerator HugSequence()
{
    // Child vanishes
    childObject.SetActive(false);

    finalMessageText.gameObject.SetActive(true);

    Color color = finalMessageText.color;
    color.a = 0f;
    finalMessageText.color = color;

        // 🔹 Fade IN
        float fadeInTime = 1.5f;
        float t = 0f;
        while (t < fadeInTime)
        {
            finalMessageText.color = new Color(
                color.r, color.g, color.b,
                Mathf.Lerp(0f, 1f, t / fadeInTime)
            );
            t += Time.deltaTime;
            yield return null;
        }

        // 🔹 HOLD
        yield return new WaitForSeconds(2f);

        // 🔹 Fade OUT
        float fadeOutTime = 2.5f;
        t = 0f;
        while (t < fadeOutTime)
        {
            finalMessageText.color = new Color(
                color.r, color.g, color.b,
                Mathf.Lerp(1f, 0f, t / fadeOutTime)
            );
            t += Time.deltaTime;
            yield return null;
        }   

        finalMessageText.gameObject.SetActive(false);
    }

}
