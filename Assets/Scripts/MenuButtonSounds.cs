using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections; // Necessário para IEnumerator para a versão com Coroutine

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RectTransform))]
public class MenuButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 0.7f;

    [Header("Y-Axis Panning Effect (2D Sound)")]
    public bool enableYPanEffect = true;
    [Tooltip("A posição Y do RectTransform (anchoredPosition.y) que corresponderá ao pan mínimo.")]
    public float yPositionForMinPan = -200f;
    [Tooltip("A posição Y do RectTransform (anchoredPosition.y) que corresponderá ao pan máximo.")]
    public float yPositionForMaxPan = 200f;
    [Tooltip("Valor mínimo do pan estéreo. Padrão: -1 (esquerda).")]
    [Range(-1f, 1f)]
    public float minPanValue = -0.8f;
    [Tooltip("Valor máximo do pan estéreo. Padrão: 1 (direita).")]
    [Range(-1f, 1f)]
    public float maxPanValue = 0.8f;

    private AudioSource localAudioSource; // Renomeado para clareza
    private Button button;
    private RectTransform rectTransform;

    void Awake()
    {
        localAudioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        localAudioSource.playOnAwake = false;
        localAudioSource.loop = false;
        localAudioSource.spatialBlend = 0; // Fundamental para panStereo funcionar
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        if (hoverSound == null) return;

        ApplyPanSettings(localAudioSource); // Aplica pan ao AudioSource local
        localAudioSource.PlayOneShot(hoverSound, soundVolume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        if (clickSound == null) return;

        // Para o som de clique, criamos um reprodutor temporário que persiste
        PlayPersistentSound(clickSound);
    }

    private void ApplyPanSettings(AudioSource audioSourceToApply)
    {
        if (enableYPanEffect)
        {
            float currentY = rectTransform.anchoredPosition.y;
            if (Mathf.Approximately(yPositionForMinPan, yPositionForMaxPan))
            {
                audioSourceToApply.panStereo = (minPanValue + maxPanValue) / 2f;
            }
            else
            {
                float t = Mathf.InverseLerp(yPositionForMinPan, yPositionForMaxPan, currentY);
                audioSourceToApply.panStereo = Mathf.Lerp(minPanValue, maxPanValue, t);
            }
        }
        else
        {
            audioSourceToApply.panStereo = 0f; // Pan centralizado
        }
    }

    private void PlayPersistentSound(AudioClip clip)
    {
        // 1. Criar um novo GameObject temporário
        GameObject soundGameObject = new GameObject("TempAudioPlayer_" + clip.name);
        AudioSource tempAudioSource = soundGameObject.AddComponent<AudioSource>();

        // 2. Configurar o AudioSource temporário
        tempAudioSource.clip = clip; // Não estritamente necessário para PlayOneShot, mas bom ter
        tempAudioSource.volume = soundVolume;
        tempAudioSource.playOnAwake = false;
        tempAudioSource.loop = false;
        tempAudioSource.spatialBlend = 0f; // Som 2D para panStereo funcionar

        // Aplicar as mesmas configurações de pan que o AudioSource local teria
        ApplyPanSettings(tempAudioSource);

        // 3. Marcar para não destruir na carga da cena
        DontDestroyOnLoad(soundGameObject);

        // 4. Tocar o som
        tempAudioSource.PlayOneShot(clip); // PlayOneShot já usa o volume do AudioSource

        // 5. Destruir o GameObject após o clipe terminar
        //    (adicionamos um pequeno buffer para garantir que terminou)
        Destroy(soundGameObject, clip.length + 0.1f);
    }

    // Se você preferir usar uma Coroutine para o Destroy (alternativa ao Destroy com delay)
    // Você precisaria chamar StartCoroutine(PlayPersistentSoundCoroutine(clip));
    /*
    private IEnumerator PlayPersistentSoundCoroutine(AudioClip clip)
    {
        GameObject soundGameObject = new GameObject("TempAudioPlayer_" + clip.name);
        AudioSource tempAudioSource = soundGameObject.AddComponent<AudioSource>();

        tempAudioSource.clip = clip;
        tempAudioSource.volume = soundVolume;
        tempAudioSource.playOnAwake = false;
        tempAudioSource.loop = false;
        tempAudioSource.spatialBlend = 0f;

        ApplyPanSettings(tempAudioSource);
        DontDestroyOnLoad(soundGameObject);
        tempAudioSource.Play(); // Usar Play() aqui se for esperar o 'yield return'

        yield return new WaitForSeconds(clip.length + 0.1f);

        Destroy(soundGameObject);
    }
    */
}