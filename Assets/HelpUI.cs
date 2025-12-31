using UnityEngine;

public class HelpUI : MonoBehaviour
{
    public GameObject helpPanel;

    public void OpenHelp()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }
}
