using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string nomeDaCena;           // Nome da cena que ser� carregada
    public AudioSource somDeClique;     // Som de clique ao trocar de cena

    public void CarregarCenaComSom()
    {
        // Toca o som de clique se estiver atribu�do
        if (somDeClique != null)
        {
            somDeClique.Play();
        }

        // Espera 1 segundo e troca de cena
        StartCoroutine(EsperarECarregarCena(1f));
    }

    // Corrotina para aguardar antes de mudar de cena
    IEnumerator EsperarECarregarCena(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        SceneManager.LoadScene(nomeDaCena); // Carrega a cena indicada
    }
}
