using UnityEngine;

public class Person : MonoBehaviour
{
    public int destino;
    [HideInInspector] public float alturaOriginalY;
    [HideInInspector] public float tempoEntrada;

    public int idParagemOrigem;  // ID da paragem onde foi gerado
    public int idDestino;        // ID da paragem de destino

    public bool destinoAtribuido = false;


    public GameObject paragemHUD;

    public void SairDoAutocarro()
    {
        gameObject.SetActive(true);
    }
}
