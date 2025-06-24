using UnityEngine;
using UnityEngine.UI;
using TMPro; // Se estiver usando TextMeshPro

public class LevelSelectItemUI : MonoBehaviour
{
    public RectTransform itemRectTransform;
    public Image backgroundImage; // Opcional, se você quiser mudar a cor/sprite também
    public TextMeshProUGUI levelNumberText; // Ou public Text levelNumberText;
    public RectTransform starsContainerRectTransform;
    public Button buttonComponent;

    void Awake()
    {
        if (itemRectTransform == null) itemRectTransform = GetComponent<RectTransform>();
        if (backgroundImage == null) backgroundImage = GetComponent<Image>();
        if (buttonComponent == null) buttonComponent = GetComponent<Button>();

        // Tente encontrar os filhos automaticamente se não foram atribuídos
        if (levelNumberText == null) levelNumberText = GetComponentInChildren<TextMeshProUGUI>(); // Ou Text
        if (starsContainerRectTransform == null)
        {
            Transform starsTF = transform.Find("StarsContainer"); // Certifique-se que o nome bate!
            if (starsTF != null)
            {
                starsContainerRectTransform = starsTF.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogWarning("StarsContainer não encontrado como filho de " + gameObject.name);
            }
        }
    }

    public void Setup(int levelNum)
    {
        if (levelNumberText != null)
        {
            levelNumberText.text = levelNum.ToString();
        }
        gameObject.name = "LevelItem_" + levelNum;
    }
}