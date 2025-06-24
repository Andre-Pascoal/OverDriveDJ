using UnityEngine;
using TMPro;

public class EndgameUI : MonoBehaviour
{
    public TMP_Text dinheiroFinalTexto;
    void Start()
    {
        // Tenta obter o nome do último nível jogado (pode ter sido "level1", "level 2", etc.)
        string levelName = PlayerPrefs.GetString("ultimoNivel", "level1");

        // Carrega o dinheiro e estrelas guardadas para esse nível
        int dinheiroFinal = SaveManager.LoadScore(levelName);

        // Atualiza o texto do dinheiro
        if (dinheiroFinalTexto != null)
            dinheiroFinalTexto.text = $"Dinheiro Ganho: ${dinheiroFinal}";
    }
}
