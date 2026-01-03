using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DisableCurrentAudioListener();
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }

    void DisableCurrentAudioListener()
    {
        // Modern API
        AudioListener listener = Object.FindFirstObjectByType<AudioListener>();
        if (listener != null)
        {
            listener.enabled = false;
        }
    }
}
