using UnityEngine;
using UnityEngine.Audio;


public class SettingsUI : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void SetEnvironmentVolume(float value)
    {
        // Slider 0–1 → dB
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("EnvironmentVolume", dB);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}