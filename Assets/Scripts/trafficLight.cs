using System.Collections;
using UnityEngine;

public class trafficLight : MonoBehaviour
{
    public enum StartOption { First, Last } //Se querem o Vermelho ou o Verde a estar ativo no Start
    public StartOption startAt = StartOption.First; //Default no vermelho

    public float startDelay = 0f; // Offset das luzes

    /*
    SINCRONIZAÇÃO:
    Caso queiram que 2 ou mais semáforos estejam sincronizados, isto é:

    Semáforo 1: Vermelho = Não pode passar
    Semáforo 2: Verde = Os que estão no sentido contrário podem passar em segurança.

    Para isto têm de usar o startDelay.

    Com o start delay, o semáforo vai começar a operar corretamente
    após x segundos definidos no startDelay.

    Para sincronizar, o startDelay tem que ter a metade da soma dos tempos
    atribuidos para a luz vermelha e verde.

    Por exemplo:
    red_delay = 10f = A luz vermelha vai aparecer por 10 segundos
    waitTime = 3f = Vai passar para o amarelo e mostrar o amarelo por 3 segundos
    green_delay = 10f = Vai passar a verde e mostrar o verde por 10 segundos

    A soma entre as duas luzes variáveis é de 20, portanto o startDelay vai ser 10f

    Isto vai fazer com que o S1 (Semáforo 1 | S2 = Semáforo 2) começe no vermelho e passado 10 segundos
    mude para o amarelo.
    O S2 vai começar o processo passados esses 10 segundos e 3 segundos antes de o S1 acabar
    os 10 segundos da luz verde, passa a amarelo.
    Quando acabar o amarelo do S2, o S2 passa a verde e o S1 passa a vermelho.
    */

    public GameObject[] lights; //array que contém as 3 luzes
    public float red_delay = 40f; //tempo que a luz vermelha vai aparecer
    public float green_delay = 40f; //tempo que a luz verde vai aparecer
    private float waitTime = 3f; //tempo que a luz amarela vai aparecer / tempo de troca entre luzes

    void Start()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null) //se existir
            {
                if (startAt == StartOption.First) //Se for First começa no vermelho
                    lights[i].SetActive(i == 0);
                else
                    lights[i].SetActive(i == lights.Length - 1); //Senão começa no verde
            }
        }

        StartCoroutine(DelayedStart()); //Delay para sincronizar
        //Se for 0 não há delay
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(ActivateInLoop()); //Processo
    }

   IEnumerator ActivateInLoop()
{
    // Espera o tempo da luz vermelha antes de iniciar o ciclo
    yield return new WaitForSeconds(red_delay);

    int length = lights.Length; // Número total de luzes (vermelho, amarelo, verde)
    int current_index;
    int previous_index;

    // Define os índices iniciais com base na cor de início escolhida (vermelho ou verde)
    if (startAt == StartOption.First)
    {
        current_index = 1;
        previous_index = 0;    // Vermelho estava ativo
    }
    else
    {
        current_index = 0;
        previous_index = length - 1;    // Verde estava ativo
    }

    // Inicia ciclo infinito
    while (true)
    {
        // Ciclo para ativar as luzes seguintes (ex: de vermelho → amarelo → verde)
        while (current_index < length)
        {
            if (lights[current_index] != null)
                lights[current_index].SetActive(true); // Ativa luz atual

            if (lights[previous_index] != null)
                lights[previous_index].SetActive(false); // Desativa luz anterior

            if (current_index == length - 1)
                break; // Se chegou à última luz (verde), sai do ciclo

            yield return new WaitForSeconds(waitTime); // Espera antes de passar à próxima luz

            previous_index = current_index; // Atualiza o índice anterior
            current_index++; // Passa à próxima luz (ex: amarelo → verde)
        }

        // A última luz (verde) está ativa agora
        yield return new WaitForSeconds(green_delay); // Espera o tempo da luz verde

        // Desativa a luz verde (última)
        if (lights[length - 1] != null)
            lights[length - 1].SetActive(false);

        // Ativa novamente a luz vermelha (primeira)
        if (lights[0] != null)
            lights[0].SetActive(true);

        yield return new WaitForSeconds(red_delay); // Espera o tempo da luz vermelha

        // Reinicia o ciclo
        current_index = 1;
        previous_index = 0;
    }
}

}
