using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // Importa para usar SceneManager

public class TimerController : MonoBehaviour
{
    [Header("Tempo inicial em segundos")]
    [SerializeField] public float tempoInicial = 300f;

    [Header("Texto de exibição")]
    [SerializeField] public TMP_Text textoTimer;

    private float tempoRestante;
    private bool contando = true;
    private bool corMudou = false;

    void Start()
    {
        tempoRestante = tempoInicial;
    }

    void Update()
    {
        if (!contando) return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            contando = false;
            TempoAcabou();
        }

        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tempoRestante % 60f);
        textoTimer.text = $"{minutos:00}:{segundos:00}";

        if (tempoRestante <= 30f && !corMudou)
        {
            textoTimer.color = Color.red;
            corMudou = true;
        }
    }

    void TempoAcabou()
    {
        Debug.Log("⏰ Tempo esgotado!");

        // Salva o dinheiro atual no PlayerPrefs
        PlayerPrefs.SetInt("DinheiroFinal", GameManager.Instance.dinheiroAtual);

        string cenaAtual = SceneManager.GetActiveScene().name;

        if (cenaAtual == "level 3")
        {
            // Se estiveres no Level 3, carrega a cena FinalCutscene
            GameManager.Instance.FinalCutscene();
        }
        else
        {
            // Senão, executa o método original (menu de derrota)
            GameManager.Instance.LostMenu();
        }
    }
}
