using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciamento de cenas

public class ChangeScene : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [Tooltip("Nome da cena para carregar. Deve corresponder exatamente ao nome do arquivo da cena (sem a extensão .unity).")]
    public string sceneNameToLoad;

    [Tooltip("Índice da cena para carregar (na Build Settings). Use isso se preferir carregar por índice em vez de nome.")]
    public int sceneIndexToLoad = -1; // -1 indica que não será usado, a menos que sceneNameToLoad esteja vazio

    // Método público que pode ser chamado por um evento de UI (como um clique de botão)
    // ou por outro script.
    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.Log($"Tentando carregar cena por nome: {sceneNameToLoad}");
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else if (sceneIndexToLoad >= 0)
        {
            // Validação básica para garantir que o índice é válido
            if (sceneIndexToLoad < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log($"Tentando carregar cena por índice: {sceneIndexToLoad}");
                SceneManager.LoadScene(sceneIndexToLoad);
            }
            else
            {
                Debug.LogError($"Índice de cena inválido: {sceneIndexToLoad}. Verifique suas Build Settings. Existem {SceneManager.sceneCountInBuildSettings} cenas na build.");
            }
        }
        else
        {
            Debug.LogError("Nenhum nome de cena ou índice de cena válido foi fornecido para carregar. Por favor, configure o script ChangeScene no Inspector.");
        }
    }

    // Sobrecarga do método para permitir carregar uma cena específica dinamicamente por nome via código
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Carregando cena por nome (via método): {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("O nome da cena fornecido para LoadSceneByName não pode ser nulo ou vazio.");
        }
    }

    // Sobrecarga do método para permitir carregar uma cena específica dinamicamente por índice via código
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Carregando cena por índice (via método): {sceneIndex}");
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Índice de cena inválido: {sceneIndex} fornecido para LoadSceneByIndex. Verifique suas Build Settings.");
        }
    }

    // Um exemplo de como você poderia adicionar uma função para sair do jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();

        // Se estiver rodando no editor do Unity, Application.Quit() não fará nada.
        // A linha abaixo para a execução no editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}