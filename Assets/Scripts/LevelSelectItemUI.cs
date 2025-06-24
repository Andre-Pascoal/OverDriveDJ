using UnityEngine;
using UnityEngine.UI;
using TMPro; // Se estiver usando TextMeshPro

public class LevelSelectItemUI : MonoBehaviour
{
    public RectTransform itemRectTransform;
    public Image backgroundImage; // Opcional, se voc� quiser mudar a cor/sprite tamb�m
    public TextMeshProUGUI levelNumberText; // Ou public Text levelNumberText;
    public RectTransform starsContainerRectTransform;
    public Button buttonComponent;

    void Awake()
    {
        if (itemRectTransform == null) itemRectTransform = GetComponent<RectTransform>();
        if (backgroundImage == null) backgroundImage = GetComponent<Image>();
        if (buttonComponent == null) buttonComponent = GetComponent<Button>();

        // Tente encontrar os filhos automaticamente se n�o foram atribu�dos
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
                Debug.LogWarning("StarsContainer n�o encontrado como filho de " + gameObject.name);
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