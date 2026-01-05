using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/MirrorDialogueData")]
public class MirrorDialogueData : ScriptableObject
{
    public string speakerName;

    [TextArea(2, 5)]
    public string[] lines;
}
