using UnityEngine;

public class LegendNPC : MonoBehaviour
{
    [Header("Legend Info")]
    public string legendName = "Legend Name";   
    public Sprite legendPortrait;               
    public DialogueLine[] dialogueLines;        

    [Header("Reward (given after dialogue ends)")]
    public Sprite rewardSprite;      // Fragment or item for this legend
    public string rewardText = "+1 Fragment";

    [Header("References")]
    public GameObject cautionSign;
    public DialogueManager dialogueManager;

    bool playerInRange = false;

    void Start()
    {
        cautionSign.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            cautionSign.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            cautionSign.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (playerInRange)
        {
            // Pass this NPC's reward data to DialogueManager
            dialogueManager.StartDialogue(
                legendName, 
                legendPortrait, 
                dialogueLines,
                rewardSprite,
                rewardText
            );
        }
    }
}
