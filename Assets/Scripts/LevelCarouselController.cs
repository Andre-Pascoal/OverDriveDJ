using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelCarouselController : MonoBehaviour
{
    [Header("Referências dos Elementos do Nível")]
    public List<RectTransform> levelButtons;
    public List<RectTransform> levelStars;

    [Header("Imagem do Nível")]
    public Image levelImageDisplay; // Objeto UI de imagem para mostrar o preview
    public List<Sprite> levelImages; // Lista de sprites para cada nível

    [Header("UI de Navegação")]
    public Button nextButtonUI;
    public Button prevButtonUI;

    [Header("Som das Setas")]
    public ArrowButtonSounds nextButtonSoundController;
    public ArrowButtonSounds prevButtonSoundController;

    [Header("Layout - Botões de Nível")]
    public Vector3 centerButtonPosition;
    public Vector2 centerButtonSize;
    public Vector3 centerButtonScale;

    public Vector3 leftButtonPosition;
    public Vector2 sideButtonSize;
    public Vector3 sideButtonScale;

    public Vector3 rightButtonPosition;

    [Header("Layout - Estrelas")]
    public Vector3 centerStarsPosition;
    public Vector2 centerStarsSize;
    public Vector3 centerStarsScale;

    public Vector3 leftStarsPosition;
    public Vector2 sideStarsSize;
    public Vector3 sideStarsScale;

    public Vector3 rightStarsPosition;

    [Header("Animação")]
    public float transitionSpeed = 8f;

    private int currentItemIndex = 0;
    private int totalItems;

    private class ItemElements
    {
        public RectTransform button;
        public RectTransform stars;

        public Vector3 targetButtonPos;
        public Vector2 targetButtonSize;
        public Vector3 targetButtonScale;

        public Vector3 targetStarsPos;
        public Vector2 targetStarsSize;
        public Vector3 targetStarsScale;

        public bool isActiveSlot;

        public void SetTargets(Vector3 btnPos, Vector2 btnSize, Vector3 btnScale,
                               Vector3 strPos, Vector2 strSize, Vector3 strScale,
                               bool active)
        {
            targetButtonPos = btnPos;
            targetButtonSize = btnSize;
            targetButtonScale = btnScale;
            targetStarsPos = strPos;
            targetStarsSize = strSize;
            targetStarsScale = strScale;
            isActiveSlot = active;

            if (button) button.gameObject.SetActive(active);
            if (stars) stars.gameObject.SetActive(active);
        }

        public void Animate(float speed)
        {
            if (!isActiveSlot || button == null || stars == null) return;

            button.anchoredPosition3D = Vector3.Lerp(button.anchoredPosition3D, targetButtonPos, Time.deltaTime * speed);
            button.sizeDelta = Vector2.Lerp(button.sizeDelta, targetButtonSize, Time.deltaTime * speed);
            button.localScale = Vector3.Lerp(button.localScale, targetButtonScale, Time.deltaTime * speed);

            stars.anchoredPosition3D = Vector3.Lerp(stars.anchoredPosition3D, targetStarsPos, Time.deltaTime * speed);
            stars.sizeDelta = Vector2.Lerp(stars.sizeDelta, targetStarsSize, Time.deltaTime * speed);
            stars.localScale = Vector3.Lerp(stars.localScale, targetStarsScale, Time.deltaTime * speed);
        }

        public void ApplyImmediate()
        {
            if (!isActiveSlot || button == null || stars == null) return;

            button.anchoredPosition3D = targetButtonPos;
            button.sizeDelta = targetButtonSize;
            button.localScale = targetButtonScale;

            stars.anchoredPosition3D = targetStarsPos;
            stars.sizeDelta = targetStarsSize;
            stars.localScale = targetStarsScale;
        }
    }

    private List<ItemElements> allItemElements = new List<ItemElements>();

    void Start()
    {
        if (levelButtons.Count != levelStars.Count || levelButtons.Count == 0)
        {
            Debug.LogError("As listas 'Level Buttons' e 'Level Stars' devem ter o mesmo número de elementos e não podem estar vazias.");
            enabled = false;
            return;
        }

        if (levelImages.Count != levelButtons.Count)
        {
            Debug.LogWarning("A lista de imagens não corresponde ao número de níveis. Verifica no Inspector.");
        }

        totalItems = levelButtons.Count;

        for (int i = 0; i < totalItems; i++)
        {
            if (levelButtons[i] == null || levelStars[i] == null)
            {
                Debug.LogError($"Elemento nulo no índice {i}.");
                enabled = false;
                return;
            }

            allItemElements.Add(new ItemElements
            {
                button = levelButtons[i],
                stars = levelStars[i]
            });

            levelButtons[i].gameObject.SetActive(false);
            levelStars[i].gameObject.SetActive(false);
        }

        if (nextButtonUI) nextButtonUI.onClick.AddListener(SelectNext);
        if (prevButtonUI) prevButtonUI.onClick.AddListener(SelectPrevious);

        currentItemIndex = 0;
        UpdateCarouselView(true);
    }

    void Update()
    {
        foreach (var itemElement in allItemElements)
        {
            if (itemElement.isActiveSlot)
                itemElement.Animate(transitionSpeed);
        }
    }

    public void SelectNext()
    {
        if (currentItemIndex < totalItems - 1)
        {
            nextButtonSoundController?.PlayClickSound();
            currentItemIndex++;
            UpdateCarouselView();
        }
    }

    public void SelectPrevious()
    {
        if (currentItemIndex > 0)
        {
            prevButtonSoundController?.PlayClickSound();
            currentItemIndex--;
            UpdateCarouselView();
        }
    }

    void UpdateCarouselView(bool immediate = false)
    {
        for (int i = 0; i < totalItems; i++)
        {
            ItemElements element = allItemElements[i];
            bool active = false;

            Vector3 btnPos = Vector3.zero;
            Vector2 btnSize = sideButtonSize;
            Vector3 btnScale = sideButtonScale;

            Vector3 strPos = Vector3.zero;
            Vector2 strSize = sideStarsSize;
            Vector3 strScale = sideStarsScale;

            if (i == currentItemIndex)
            {
                active = true;
                btnPos = centerButtonPosition;
                btnSize = centerButtonSize;
                btnScale = centerButtonScale;

                strPos = centerStarsPosition;
                strSize = centerStarsSize;
                strScale = centerStarsScale;
            }
            else if (i == currentItemIndex - 1)
            {
                active = true;
                btnPos = leftButtonPosition;
                strPos = leftStarsPosition;
            }
            else if (i == currentItemIndex + 1)
            {
                active = true;
                btnPos = rightButtonPosition;
                strPos = rightStarsPosition;
            }

            element.SetTargets(btnPos, btnSize, btnScale, strPos, strSize, strScale, active);

            if (immediate)
                element.ApplyImmediate();
        }

        UpdateLevelImage();
    }

    void UpdateLevelImage()
    {
        if (levelImageDisplay != null && currentItemIndex >= 0 && currentItemIndex < levelImages.Count)
        {
            levelImageDisplay.sprite = levelImages[currentItemIndex];
        }
    }
}
