using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class ParagemComMascara : MonoBehaviour
{
    [Header("Configuração de Cores")]
    public Color corVerde = Color.green;
    public Color corAmarelo = Color.yellow;
    public Color corVermelho = Color.red;

    [Header("Tempo")]
    public float tempoTotal = 240f;

    private RectTransform innerContainer;
    private HorizontalLayoutGroup layoutGroup;
    private int numBlocos = 0;
    private float tempoPorBloco;

    private int estadoAtual = 0; // 0 = verde, 1 = amarelo, 2 = vermelho

    [Header("Número da Paragem")]
    public TMP_Text textoNumeroParagem; // ou public Text textoNumeroParagem;

    public int idParagemHUD; // Guarda o valor atual visível

    [Header("Satisfação")]
    public int satisfacao = 3; // Podes ajustar conforme o desempenho ou tempo


    void Start()
    {
        Transform maskContainer = transform.Find("IndicadoresMaskContainer");
        if (maskContainer == null)
        {
            Debug.LogError("IndicadoresMaskContainer não encontrado!");
            return;
        }

        innerContainer = maskContainer.Find("IndicadoresInnerContainer")?.GetComponent<RectTransform>();
        if (innerContainer == null)
        {
            Debug.LogError("IndicadoresInnerContainer não encontrado!");
            return;
        }

        numBlocos = innerContainer.childCount;
        tempoPorBloco = tempoTotal / numBlocos;

        // Começa com todos em verde
        AtualizarCor(corVerde);

        StartCoroutine(IniciarAnimacaoMascara());
    }

    IEnumerator IniciarAnimacaoMascara()
    {
        float larguraBloco = innerContainer.GetChild(0).GetComponent<RectTransform>().rect.width;
        float deslocamentoTotal = larguraBloco * numBlocos;

        for (int i = 0; i < numBlocos; i++)
        {
            float tempo = 0f;
            Vector2 posInicial = innerContainer.anchoredPosition;
            Vector2 posFinal = posInicial + new Vector2(-larguraBloco, 0f);

            while (tempo < tempoPorBloco)
            {
                tempo += Time.deltaTime;
                float t = Mathf.Clamp01(tempo / tempoPorBloco);
                innerContainer.anchoredPosition = Vector2.Lerp(posInicial, posFinal, t);
                yield return null;
            }

            // Atualiza cor dos restantes blocos
            estadoAtual++;
            AtualizarCorParaEstadoAtual(i + 1);
        }

        Destroy(gameObject);
    }

    void AtualizarCor(Color novaCor, int startIndex = 0)
    {
        for (int i = startIndex; i < innerContainer.childCount; i++)
        {
            Image img = innerContainer.GetChild(i).GetComponent<Image>();
            if (img != null)
                img.color = novaCor;
        }
    }

    void AtualizarCorParaEstadoAtual(int startIndex)
    {
        switch (estadoAtual)
        {
            case 1:
                AtualizarCor(corAmarelo, startIndex);
                break;
            case 2:
                AtualizarCor(corVermelho, startIndex);
                break;
        }
    }

    public void DefinirNumero(int numero)
    {
        idParagemHUD = numero;
        if (textoNumeroParagem != null)
        {
            textoNumeroParagem.text = numero.ToString();
        }
    }
}