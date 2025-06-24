using UnityEngine;

public class BusCameraFollow : MonoBehaviour
{
    public Transform bus;        // Referência ao autocarro
    public Vector3 offset;       // Offset inicial da câmara
    public float smoothSpeed = 0.1f; // Suavidade da transição
    public float strafeAmount = 5f;  // Quanto a câmara desliza para os lados

    void LateUpdate()
    {
        // Obtém o input lateral (A/D ou setas esquerda/direita)
        float moveX = Input.GetAxis("Horizontal");

        // Calcula o novo offset com o pequeno strafe lateral
        Vector3 strafeOffset = transform.right * moveX * strafeAmount;

        // Posição desejada da câmara
        Vector3 desiredPosition = bus.position + offset + strafeOffset;

        // Suaviza o movimento da câmara para não ficar brusco
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Mantém a câmara sempre focada no autocarro
        transform.LookAt(bus);
    }
}
