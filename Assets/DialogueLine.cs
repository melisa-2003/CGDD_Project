using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string text;    // Dialogue text
    public Sprite portrait;            // Optional: override portrait for this line
}
