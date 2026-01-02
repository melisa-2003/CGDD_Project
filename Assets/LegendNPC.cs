using UnityEngine;

public class LegendNPC : MonoBehaviour
{
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

    void OnMouseDown()   // Mobile tap works if NPC has collider
    {
        if (playerInRange)
        {
            dialogueManager.StartHangTuahDialogue();
        }
    }
}
