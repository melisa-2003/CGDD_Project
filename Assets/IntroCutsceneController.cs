using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroCutsceneController : MonoBehaviour
{
    [Header("Cutscene Objects")]
    public VideoPlayer videoPlayer;
    public GameObject canvasVideo;
    public GameObject canvasText;
    public PlayableDirector timelineDirector;
    public float blinkInterval = 0.5f;

    private bool videoTransitioned = false;
    private bool timelineTransitioned = false;


    [Header("Fade Settings")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 0.7f;

    [Header("Skip UI")]
    public GameObject skipTextCanvas; // Separate canvas for "Tap Anywhere to Skip"

    [Header("Next Scene")]
    public string nextSceneName = "Purity"; // Name of your target scene

    private bool transitioned = false;
    private Coroutine timelineCoroutine;

    void Start()
    {
        if (skipTextCanvas != null)
            skipTextCanvas.SetActive(true);
    }

    void Update()
    {
        // Tap to skip during video OR timeline
        if (( !videoTransitioned && videoPlayer.isPlaying ) || ( !timelineTransitioned && timelineDirector.state == PlayState.Playing ))
        {
            if (Input.touchCount > 0 || Input.anyKeyDown)
            {
                SkipEverything();
            }
        }

        // Auto-start timeline after video ends
        if (!videoTransitioned && !videoPlayer.isPlaying && videoPlayer.time > 0)
        {
            StartTimelineSequence();
        }
    }

    void SkipEverything()
    {
        // Skip video if playing
        if (!videoTransitioned && videoPlayer.isPlaying)
            {
                videoTransitioned = true;
                videoPlayer.Stop();
            }
        // Skip timeline if playing
        if (!timelineTransitioned && (timelineDirector.state == PlayState.Playing || timelineDirector.time < timelineDirector.duration))
        {
            timelineTransitioned = true;
            timelineDirector.time = timelineDirector.duration;
            timelineDirector.Evaluate();
            timelineDirector.Stop();
        }

        // Stop ongoing coroutine if any
        if (timelineCoroutine != null)
            StopCoroutine(timelineCoroutine);

        // Immediately fade and load next scene
        StartCoroutine(FadeAndLoadNext());
    }

    void StartTimelineSequence()
    {
        if (timelineTransitioned) return; // Only start if not already transitioned

        timelineCoroutine = StartCoroutine(FadePlayTimeline());
    }

    IEnumerator FadePlayTimeline()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 1;

        canvasVideo.SetActive(false);
        canvasText.SetActive(true);

        timelineDirector.Play();

        yield return new WaitForSeconds((float)timelineDirector.duration);

        StartCoroutine(FadeAndLoadNext());
    }

    IEnumerator FadeAndLoadNext()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 1;

        // Load the separate scene "Purity"
        SceneManager.LoadScene(nextSceneName);
    }
    
}
