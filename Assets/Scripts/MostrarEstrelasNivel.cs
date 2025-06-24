using UnityEngine;
using UnityEngine.UI;

public class MostrarEstrelasNivel : MonoBehaviour
{
    public string nomeNivel; // Ex: "Level 1"
    public Image[] estrelas;
    public Color corAtiva = Color.yellow;
    public Color corInativa = Color.gray;

    void Start()
    {
        int estrelasGuardadas = SaveManager.LoadStars(nomeNivel);
        AtualizarEstrelas(estrelasGuardadas);
    }

    void AtualizarEstrelas(int quantidade)
    {
        for (int i = 0; i < estrelas.Length; i++)
        {
            estrelas[i].color = (i < quantidade) ? corAtiva : corInativa;
        }
    }
}
