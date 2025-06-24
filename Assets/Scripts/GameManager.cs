using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public TMP_Text dinheiroTexto;
    public int dinheiroAtual = 0;

    [Header("Painéis")]
    public GameObject settingsPanelUI;
    public GameObject pauseMenuUI;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Mixer")]
    public AudioMixer mainMixer;

    [Header("Fontes de Áudio")]
    public AudioSource musicSource;
    public AudioSource sfxSource; // 🎧 Fonte para efeitos sonoros
    public AudioClip pauseOpenSound; // 🔊 Som ao abrir o pause

    private bool isPaused = false;
    private bool canTogglePause = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.volume = 1f;
        musicSlider.value = 1.0f;
        sfxSlider.value = 1.0f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.loop = true;
            musicSource.Play();
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canTogglePause)
        {
            TogglePauseMenu();
        }


    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            Debug.Log("🎵 musicSource.Pause() chamado");
            musicSource.Pause();

            // 🔊 Toca som de abrir o menu de pausa
            if (pauseOpenSound != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(pauseOpenSound);
            }
        }
        else
        {
            Debug.Log("🎵 musicSource.UnPause() chamado");
            musicSource.UnPause();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("🎵 ResumeGame → musicSource.UnPause()");
        musicSource.UnPause();

        StartCoroutine(BlockEscapeTemporariamente());
    }

    private IEnumerator BlockEscapeTemporariamente()
    {
        canTogglePause = false;
        yield return new WaitForSecondsRealtime(0.2f);
        canTogglePause = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void LostMenu()
    {
        GuardarProgresso();
        SceneManager.LoadScene("Endgame");
    }

    public void FinalCutscene()
    {
        GuardarProgresso();
        SceneManager.LoadScene("FinalCutscene");
    }

    public void OpenSettings()
    {
        settingsPanelUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanelUI.SetActive(false);
    }

    public void SetMusicVolume(float value)
    {
        float safeValue = Mathf.Clamp(value, 0.0001f, 1f);
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(safeValue) * 20);
    }

    public void SetSFXVolume(float value)
    {
        float safeValue = Mathf.Clamp(value, 0.0001f, 1f);
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(safeValue) * 20);
    }

    public void AdicionarDinheiro(int satisfacao)
    {
        int ganho = satisfacao * 50;
        dinheiroAtual += ganho;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (dinheiroTexto != null)
        {
            dinheiroTexto.text = $"Dinheiro: ${dinheiroAtual}";
        }
    }

    public void GuardarProgresso()
    {
        string levelName = SceneManager.GetActiveScene().name;

        // Guardar o nome do último nível jogado
        PlayerPrefs.SetString("ultimoNivel", levelName);

        // Guardar o dinheiro ganho neste nível
        SaveManager.SaveScore(levelName, dinheiroAtual);

        // Calcular e guardar número de estrelas
        int estrelas = CalcularEstrelas(levelName, dinheiroAtual);
        SaveManager.SaveStars(levelName, estrelas);

        Debug.Log($"✅ Progresso guardado: {levelName} - {dinheiroAtual}€ - {estrelas} estrelas");
    }


    int CalcularEstrelas(string levelName, int dinheiro)
    {
        switch (levelName)
        {
            case "level1":
                if (dinheiro >= 1000) return 3;
                if (dinheiro >= 750) return 2;
                if (dinheiro >= 500) return 1;
                break;

            case "level 2":
                if (dinheiro >= 1250) return 3;
                if (dinheiro >= 1000) return 2;
                if (dinheiro >= 750) return 1;
                break;

            case "level 3":
                if (dinheiro >= 1500) return 3;
                if (dinheiro >= 1250) return 2;
                if (dinheiro >= 1000) return 1;
                break;
        }
        return 0;
    }
}
