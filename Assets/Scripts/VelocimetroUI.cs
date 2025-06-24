using UnityEngine;
using UnityEngine.UI;

public class VelocimetroUI : MonoBehaviour
{
    [Header("Imagem interna com preenchimento radial")]
    public Image velocimetroImage; // Image com fill radial

    [Header("Velocidade máxima esperada")]
    public float velocidadeMaxima = 100f;

    [Header("Referência ao carro")]
    public PrometeoCarController carro; // arrasta o carro aqui no inspetor

    void Update()
    {
        if (carro != null)
        {
            float velocidadeAtual = Mathf.Abs(carro.carSpeed);
            float valorFill = Mathf.Clamp01(velocidadeAtual / velocidadeMaxima);
            velocimetroImage.fillAmount = valorFill;
        }
    }
}
