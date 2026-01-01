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
    public float fadeDuration = 0.35f;

    [Header("Skip UI")]
    public GameObject skipTextCanvas; // Separate canvas for "Tap Anywhere to Skip"

    [Header("Next Scene")]
    public string nextSceneName = "Purity"; // Name of your target scene

    [Header("Intro BGM")]
    public AudioSource introBGM;
    public float bgmFadeDuration = 2f;
    public float bgmTargetVolume = 0.6f;
    
    private Coroutine timelineCoroutine;

    void Start()
    {
        fadeGroup.alpha = 1f;
        fadeGroup.blocksRaycasts = true;

        canvasText.SetActive(false);
        canvasVideo.SetActive(true);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        StartCoroutine(FadeInFromBlack());
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.anyKeyDown)
        {
            SkipEverything();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (videoTransitioned) return;

        videoTransitioned = true;
        Debug.Log("VIDEO REALLY FINISHED at time: " + vp.time);
        StartCoroutine(FadePlayTimeline());
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

        // Fade out BGM before fading to next scene
        StartCoroutine(FadeOutBGM());  
        // Immediately fade and load next scene
        StartCoroutine(FadeAndLoadNext());
    }

    void StartTimelineSequence()
    {
        if (timelineTransitioned) return; // Only start if not already transitioned

        timelineCoroutine = StartCoroutine(FadePlayTimeline());
    }


    IEnumerator FadeInFromBlack()
    {
        fadeGroup.blocksRaycasts = true; // 一开始挡住点击
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
    }

    IEnumerator FadePlayTimeline()
    {
        float t = 0f;

        // fade to black
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 1f;

        // contents switch
        canvasVideo.SetActive(false);
        canvasText.SetActive(true);
        timelineDirector.Play();

        StartCoroutine(FadeInBGM());
        
        // fade out from black
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;

        Debug.Log("END FadePlayTimeline at Time.time = " + Time.time);
    }

    IEnumerator FadeInBGM()
    {
        if (introBGM == null) {
            yield break;
        }

        introBGM.volume = 0f;
        introBGM.Play();


        float t = 0f;
        while (t < bgmFadeDuration)
        {
            t += Time.deltaTime;
            introBGM.volume = Mathf.Lerp(0f, bgmTargetVolume, t / bgmFadeDuration);
            yield return null;
        }
        introBGM.volume = bgmTargetVolume;
    }

    IEnumerator FadeOutBGM()
    {
        if (introBGM == null) yield break;

        float startVolume = introBGM.volume;
        float t = 0f;

        while (t < bgmFadeDuration)
        {
            t += Time.deltaTime;
            introBGM.volume = Mathf.Lerp(startVolume, 0f, t / bgmFadeDuration);
            yield return null;
        }
        introBGM.volume = 0f;
        introBGM.Stop();
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
