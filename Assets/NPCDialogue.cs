using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;

    [TextArea]
    public string message = "The brave guardian will protect us… as long as we believe he's still here. The shadows are getting closer...";

    void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueText.text = message;
            dialogueCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(false);
        }
    }
}
