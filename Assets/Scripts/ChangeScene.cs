using UnityEngine;
using UnityEngine.SceneManagement; // Necess�rio para gerenciamento de cenas

public class ChangeScene : MonoBehaviour
{
    [Header("Configura��o da Cena")]
    [Tooltip("Nome da cena para carregar. Deve corresponder exatamente ao nome do arquivo da cena (sem a extens�o .unity).")]
    public string sceneNameToLoad;

    [Tooltip("�ndice da cena para carregar (na Build Settings). Use isso se preferir carregar por �ndice em vez de nome.")]
    public int sceneIndexToLoad = -1; // -1 indica que n�o ser� usado, a menos que sceneNameToLoad esteja vazio

    // M�todo p�blico que pode ser chamado por um evento de UI (como um clique de bot�o)
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
            // Valida��o b�sica para garantir que o �ndice � v�lido
            if (sceneIndexToLoad < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log($"Tentando carregar cena por �ndice: {sceneIndexToLoad}");
                SceneManager.LoadScene(sceneIndexToLoad);
            }
            else
            {
                Debug.LogError($"�ndice de cena inv�lido: {sceneIndexToLoad}. Verifique suas Build Settings. Existem {SceneManager.sceneCountInBuildSettings} cenas na build.");
            }
        }
        else
        {
            Debug.LogError("Nenhum nome de cena ou �ndice de cena v�lido foi fornecido para carregar. Por favor, configure o script ChangeScene no Inspector.");
        }
    }

    // Sobrecarga do m�todo para permitir carregar uma cena espec�fica dinamicamente por nome via c�digo
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"Carregando cena por nome (via m�todo): {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("O nome da cena fornecido para LoadSceneByName n�o pode ser nulo ou vazio.");
        }
    }

    // Sobrecarga do m�todo para permitir carregar uma cena espec�fica dinamicamente por �ndice via c�digo
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Carregando cena por �ndice (via m�todo): {sceneIndex}");
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"�ndice de cena inv�lido: {sceneIndex} fornecido para LoadSceneByIndex. Verifique suas Build Settings.");
        }
    }

    // Um exemplo de como voc� poderia adicionar uma fun��o para sair do jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();

        // Se estiver rodando no editor do Unity, Application.Quit() n�o far� nada.
        // A linha abaixo para a execu��o no editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}