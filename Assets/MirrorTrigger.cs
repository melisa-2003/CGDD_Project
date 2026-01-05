using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    [Header("Mirror Sprites")]
    public Sprite normalMirror;
    public Sprite crackedMirror;

    [Header("Dialogue")]
    public MirrorDialogueData mirrorDialogue;
    public MirrorDialogueManager dialogueManager;

    private SpriteRenderer sr;
    private bool triggered = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = normalMirror;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Start the dialogue and run OnDialogueFinished after it ends
            dialogueManager.StartDialogue(mirrorDialogue, OnDialogueFinished);
        }
    }

    private void OnDialogueFinished()
    {
        if (sr != null)
            sr.sprite = crackedMirror;
    }
}
