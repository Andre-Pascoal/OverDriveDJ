using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance;

    [Header("Configurações da Música")]
    public AudioClip backgroundMusicClip;
    [Range(0f, 1f)]
    public float musicVolume = 0.2f;

    [Header("Cenas Onde a Música NÃO Deve Tocar")]
    public List<string> scenesToMuteMusic;

    private AudioSource audioSource;
    private bool musicShouldBePlaying = true;
    private float originalVolume;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado!");
            enabled = false;
            return;
        }

        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = musicVolume;
        originalVolume = musicVolume;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        CheckSceneAndPlayMusic(SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndPlayMusic(scene);
    }

    void CheckSceneAndPlayMusic(Scene currentScene)
    {
        if (audioSource == null || audioSource.clip == null) return;

        if (scenesToMuteMusic.Contains(currentScene.name))
        {
            musicShouldBePlaying = false;
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            musicShouldBePlaying = true;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
            originalVolume = musicVolume;
        }
    }

    public void ToggleMusic(bool play)
    {
        if (audioSource == null) return;

        if (play && musicShouldBePlaying)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Pause();
        }
    }

    public bool IsMusicPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }

    // NOVO: fade out + stop
    public void FadeOutAndStop(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = originalVolume;
    }
}
