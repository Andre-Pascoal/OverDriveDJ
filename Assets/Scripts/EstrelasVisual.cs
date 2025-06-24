using UnityEngine;
using UnityEngine.UI;

public class EstrelasVisual : MonoBehaviour
{
    public Image[] estrelas; // As 3 imagens de estrelas
    public Color corAtiva = Color.yellow;
    public Color corInativa = Color.gray;

    void Start()
    {
        string levelName = PlayerPrefs.GetString("ultimoNivel", "level1");
        int estrelasGanhas = SaveManager.LoadStars(levelName);

        AtualizarEstrelas(estrelasGanhas);
    }

    public void AtualizarEstrelas(int quantidade)
    {
        for (int i = 0; i < estrelas.Length; i++)
        {
            estrelas[i].color = (i < quantidade) ? corAtiva : corInativa;
        }
    }
}
