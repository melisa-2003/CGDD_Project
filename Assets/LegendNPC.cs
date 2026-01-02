using UnityEngine;

public class LegendNPC : MonoBehaviour
{
    [Header("Legend Info")]
    public string legendName = "Legend Name";   
    public Sprite legendPortrait;               
    public DialogueLine[] dialogueLines;        

    [Header("Reward (given after dialogue ends)")]
    public Sprite rewardSprite;      
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

    public void TriggerDialogue()
    {
        if (playerInRange)
        {
            dialogueManager.StartDialogue(
                legendName, 
                legendPortrait, 
                dialogueLines,
                rewardSprite,
                rewardText
            );
        }
    }

    void Update()
    {
        // Mobile touch
        if (playerInRange && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D hit = Physics2D.OverlapPoint(worldPos);
                if (hit != null && hit.gameObject == gameObject)
                {
                    TriggerDialogue();
                }
            }
        }

        // PC mouse click
        if (playerInRange && Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);
            if (hit != null && hit.gameObject == gameObject)
            {
                TriggerDialogue();
            }
        }
    }
}
