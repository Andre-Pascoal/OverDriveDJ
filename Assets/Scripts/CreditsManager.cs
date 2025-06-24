using UnityEngine;
using UnityEngine.UI; // Se estiver usando UI Text legado
using TMPro;         // Se estiver usando TextMeshPro (recomendado)
using System.Collections;
using System.Collections.Generic; // Para usar List<string>

// Define a estrutura para cada secção de créditos
[System.Serializable] // Isso permite que seja editável no Inspector do Unity
public class CreditSection
{
    public string titulo; // Ex: "Criado por", "Artistas", "Programadores"
    [TextArea(3, 10)] // Faz o campo de texto ser maior no Inspector
    public string nomes;  // Nomes separados por quebra de linha
}

public class CreditsManager : MonoBehaviour
{
    [Header("Referências aos Elementos de UI")]
    // Use TextMeshProUGUI se estiver usando TextMeshPro, ou Text para UI Text legado
    public TextMeshProUGUI textoEmbaixoLogoUI;
    public TextMeshProUGUI nomesUI;

    [Header("Configurações dos Créditos")]
    public float tempoPorSecao = 3f; // Tempo que cada secção fica visível
    public List<CreditSection> secoesDeCreditos = new List<CreditSection>();

    private int secaoAtualIndex = 0;
    private Coroutine loopCreditosCoroutine;

    void Start()
    {
        // Validação inicial
        if (textoEmbaixoLogoUI == null || nomesUI == null)
        {
            Debug.LogError("Por favor, atribua os componentes de Texto (TextMeshProUGUI ou Text) no Inspector!");
            enabled = false; // Desabilita o script se não houver referências
            return;
        }

        if (secoesDeCreditos.Count == 0)
        {
            Debug.LogWarning("Nenhuma secção de créditos foi definida. Adicione algumas no Inspector.");
            // Poderia adicionar uma secção padrão aqui se quisesse
            // secoesDeCreditos.Add(new CreditSection { titulo = "Sem Créditos", nomes = "Adicione seções no Inspector" });
            // enabled = false; // Ou desabilitar
            // return;
        }

        // Inicia o loop de créditos
        loopCreditosCoroutine = StartCoroutine(LoopDeCreditos());
    }

    IEnumerator LoopDeCreditos()
    {
        // Loop infinito para exibir os créditos
        while (true)
        {
            if (secoesDeCreditos.Count == 0)
            {
                // Se por algum motivo a lista ficar vazia durante a execução
                textoEmbaixoLogoUI.text = "Erro";
                nomesUI.text = "Nenhuma secção de créditos.";
                yield return new WaitForSeconds(tempoPorSecao); // Espera para não sobrecarregar
                continue; // Volta ao início do while
            }

            // Garante que o índice está dentro dos limites
            secaoAtualIndex = secaoAtualIndex % secoesDeCreditos.Count;

            // Pega a secção atual
            CreditSection secaoAtual = secoesDeCreditos[secaoAtualIndex];

            // Atualiza os textos na UI
            textoEmbaixoLogoUI.text = secaoAtual.titulo;
            nomesUI.text = secaoAtual.nomes; // Os nomes já vêm formatados com quebras de linha do Inspector

            // Espera o tempo definido
            yield return new WaitForSeconds(tempoPorSecao);

            // Avança para a próxima secção
            secaoAtualIndex++;
        }
    }

    // Opcional: se você precisar parar ou reiniciar os créditos de outro script
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

    // Chamado quando o objeto é destruído ou a cena descarregada
    void OnDestroy()
    {
        PararCreditos();
    }
}