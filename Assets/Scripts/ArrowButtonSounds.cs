using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RectTransform))]
public class ArrowButtonSounds : MonoBehaviour, IPointerEnterHandler // Removido IPointerClickHandler
{
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound; // Este será usado pelo método público

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 0.7f;

    [Header("X-Axis Panning Effect (2D Sound)")]
    public bool enableXPanEffect = true;

    [Tooltip("A posição X do RectTransform (anchoredPosition.x) que corresponderá ao pan mínimo.")]
    public float xPositionForMinPan = -100f;

    [Tooltip("A posição X do RectTransform (anchoredPosition.x) que corresponderá ao pan máximo.")]
    public float xPositionForMaxPan = 100f;

    [Tooltip("Valor mínimo do pan estéreo. Padrão: -1 (esquerda).")]
    [Range(-1f, 1f)]
    public float minPanValue = -0.8f;

    [Tooltip("Valor máximo do pan estéreo. Padrão: 1 (direita).")]
    [Range(-1f, 1f)]
    public float maxPanValue = 0.8f;

    private AudioSource localAudioSource;
    private Button buttonComponent; // Renomeado para evitar confusão com os botões do carrossel
    private RectTransform rectTransform;

    void Awake()
    {
        localAudioSource = GetComponent<AudioSource>();
        buttonComponent = GetComponent<Button>(); // Pega o componente Button deste GameObject
        rectTransform = GetComponent<RectTransform>();

        if (localAudioSource == null)
        {
            Debug.LogError("ArrowButtonSounds: AudioSource não encontrado!", gameObject);
            enabled = false;
            return;
        }
        if (rectTransform == null)
        {
            Debug.LogError("ArrowButtonSounds: RectTransform não encontrado!", gameObject);
            enabled = false;
            return;
        }

        localAudioSource.playOnAwake = false;
        localAudioSource.loop = false;
        localAudioSource.spatialBlend = 0; // Fundamental para panStereo funcionar
    }

    // Chamado quando o cursor do mouse entra na área do botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Verifica se o botão associado a ESTE script está interativo
        if (buttonComponent != null && !buttonComponent.interactable)
        {
            // Debug.Log($"Hover ignorado em {gameObject.name}, botão não interativo.");
            return;
        }

        if (hoverSound != null && localAudioSource != null)
        {
            ApplyPanSettings(localAudioSource);
            localAudioSource.PlayOneShot(hoverSound, soundVolume);
        }
        else if (hoverSound == null)
        {
            // Debug.LogWarning($"Hover sound não definido para {gameObject.name}");
        }
    }

    // Método PÚBLICO para ser chamado pelo LevelCarouselController
    public void PlayClickSound()
    {
        if (clickSound == null)
        {
            Debug.LogWarning($"Click sound não definido para {gameObject.name}, não é possível tocar.");
            return;
        }
        if (localAudioSource == null)
        {
            Debug.LogError($"localAudioSource é nulo em {gameObject.name} ao tentar tocar som de clique.");
            return;
        }

        ApplyPanSettings(localAudioSource);
        localAudioSource.PlayOneShot(clickSound, soundVolume);
        // Debug.Log($"PlayClickSound chamado para {gameObject.name}");
    }

    private void ApplyPanSettings(AudioSource audioSourceToApply)
    {
        if (!enableXPanEffect || audioSourceToApply == null || rectTransform == null) return;

        float currentX = rectTransform.anchoredPosition.x;

        if (Mathf.Approximately(xPositionForMinPan, xPositionForMaxPan))
        {
            audioSourceToApply.panStereo = (minPanValue + maxPanValue) / 2f;
        }
        else
        {
            float t = Mathf.InverseLerp(xPositionForMinPan, xPositionForMaxPan, currentX);
            audioSourceToApply.panStereo = Mathf.Lerp(minPanValue, maxPanValue, t);
        }
    }
}