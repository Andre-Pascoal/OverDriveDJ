using UnityEngine;
using UnityEngine.UI; // Se estiver usando UI Text legado
using TMPro;         // Se estiver usando TextMeshPro (recomendado)
using System.Collections;
using System.Collections.Generic; // Para usar List<string>

// Define a estrutura para cada sec��o de cr�ditos
[System.Serializable] // Isso permite que seja edit�vel no Inspector do Unity
public class CreditSection
{
    public string titulo; // Ex: "Criado por", "Artistas", "Programadores"
    [TextArea(3, 10)] // Faz o campo de texto ser maior no Inspector
    public string nomes;  // Nomes separados por quebra de linha
}

public class CreditsManager : MonoBehaviour
{
    [Header("Refer�ncias aos Elementos de UI")]
    // Use TextMeshProUGUI se estiver usando TextMeshPro, ou Text para UI Text legado
    public TextMeshProUGUI textoEmbaixoLogoUI;
    public TextMeshProUGUI nomesUI;

    [Header("Configura��es dos Cr�ditos")]
    public float tempoPorSecao = 3f; // Tempo que cada sec��o fica vis�vel
    public List<CreditSection> secoesDeCreditos = new List<CreditSection>();

    private int secaoAtualIndex = 0;
    private Coroutine loopCreditosCoroutine;

    void Start()
    {
        // Valida��o inicial
        if (textoEmbaixoLogoUI == null || nomesUI == null)
        {
            Debug.LogError("Por favor, atribua os componentes de Texto (TextMeshProUGUI ou Text) no Inspector!");
            enabled = false; // Desabilita o script se n�o houver refer�ncias
            return;
        }

        if (secoesDeCreditos.Count == 0)
        {
            Debug.LogWarning("Nenhuma sec��o de cr�ditos foi definida. Adicione algumas no Inspector.");
            // Poderia adicionar uma sec��o padr�o aqui se quisesse
            // secoesDeCreditos.Add(new CreditSection { titulo = "Sem Cr�ditos", nomes = "Adicione se��es no Inspector" });
            // enabled = false; // Ou desabilitar
            // return;
        }

        // Inicia o loop de cr�ditos
        loopCreditosCoroutine = StartCoroutine(LoopDeCreditos());
    }

    IEnumerator LoopDeCreditos()
    {
        // Loop infinito para exibir os cr�ditos
        while (true)
        {
            if (secoesDeCreditos.Count == 0)
            {
                // Se por algum motivo a lista ficar vazia durante a execu��o
                textoEmbaixoLogoUI.text = "Erro";
                nomesUI.text = "Nenhuma sec��o de cr�ditos.";
                yield return new WaitForSeconds(tempoPorSecao); // Espera para n�o sobrecarregar
                continue; // Volta ao in�cio do while
            }

            // Garante que o �ndice est� dentro dos limites
            secaoAtualIndex = secaoAtualIndex % secoesDeCreditos.Count;

            // Pega a sec��o atual
            CreditSection secaoAtual = secoesDeCreditos[secaoAtualIndex];

            // Atualiza os textos na UI
            textoEmbaixoLogoUI.text = secaoAtual.titulo;
            nomesUI.text = secaoAtual.nomes; // Os nomes j� v�m formatados com quebras de linha do Inspector

            // Espera o tempo definido
            yield return new WaitForSeconds(tempoPorSecao);

            // Avan�a para a pr�xima sec��o
            secaoAtualIndex++;
        }
    }

    // Opcional: se voc� precisar parar ou reiniciar os cr�ditos de outro script
    public void PararCreditos()
    {
        if (loopCreditosCoroutine != null)
        {
            StopCoroutine(loopCreditosCoroutine);
        }
    }

    public void ReiniciarCreditos()
    {
        PararCreditos();
        secaoAtualIndex = 0;
        loopCreditosCoroutine = StartCoroutine(LoopDeCreditos());
    }

    // Chamado quando o objeto � destru�do ou a cena descarregada
    void OnDestroy()
    {
        PararCreditos();
    }
}