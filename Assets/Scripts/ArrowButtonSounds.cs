using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RectTransform))]
public class ArrowButtonSounds : MonoBehaviour, IPointerEnterHandler // Removido IPointerClickHandler
{
    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound; // Este ser� usado pelo m�todo p�blico

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 0.7f;

    [Header("X-Axis Panning Effect (2D Sound)")]
    public bool enableXPanEffect = true;

    [Tooltip("A posi��o X do RectTransform (anchoredPosition.x) que corresponder� ao pan m�nimo.")]
    public float xPositionForMinPan = -100f;

    [Tooltip("A posi��o X do RectTransform (anchoredPosition.x) que corresponder� ao pan m�ximo.")]
    public float xPositionForMaxPan = 100f;

    [Tooltip("Valor m�nimo do pan est�reo. Padr�o: -1 (esquerda).")]
    [Range(-1f, 1f)]
    public float minPanValue = -0.8f;

    [Tooltip("Valor m�ximo do pan est�reo. Padr�o: 1 (direita).")]
    [Range(-1f, 1f)]
    public float maxPanValue = 0.8f;

    private AudioSource localAudioSource;
    private Button buttonComponent; // Renomeado para evitar confus�o com os bot�es do carrossel
    private RectTransform rectTransform;

    void Awake()
    {
        localAudioSource = GetComponent<AudioSource>();
        buttonComponent = GetComponent<Button>(); // Pega o componente Button deste GameObject
        rectTransform = GetComponent<RectTransform>();

        if (localAudioSource == null)
        {
            Debug.LogError("ArrowButtonSounds: AudioSource n�o encontrado!", gameObject);
            enabled = false;
            return;
        }
        if (rectTransform == null)
        {
            Debug.LogError("ArrowButtonSounds: RectTransform n�o encontrado!", gameObject);
            enabled = false;
            return;
        }

        localAudioSource.playOnAwake = false;
        localAudioSource.loop = false;
        localAudioSource.spatialBlend = 0; // Fundamental para panStereo funcionar
    }

    // Chamado quando o cursor do mouse entra na �rea do bot�o
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Verifica se o bot�o associado a ESTE script est� interativo
        if (buttonComponent != null && !buttonComponent.interactable)
        {
            // Debug.Log($"Hover ignorado em {gameObject.name}, bot�o n�o interativo.");
            return;
        }

        if (hoverSound != null && localAudioSource != null)
        {
            ApplyPanSettings(localAudioSource);
            localAudioSource.PlayOneShot(hoverSound, soundVolume);
        }
        else if (hoverSound == null)
        {
            // Debug.LogWarning($"Hover sound n�o definido para {gameObject.name}");
        }
    }

    // M�todo P�BLICO para ser chamado pelo LevelCarouselController
    public void PlayClickSound()
    {
        if (clickSound == null)
        {
            Debug.LogWarning($"Click sound n�o definido para {gameObject.name}, n�o � poss�vel tocar.");
            return;
        }
        if (localAudioSource == null)
        {
            Debug.LogError($"localAudioSource � nulo em {gameObject.name} ao tentar tocar som de clique.");
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