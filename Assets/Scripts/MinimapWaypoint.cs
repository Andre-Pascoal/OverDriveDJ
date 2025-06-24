using UnityEngine;

public class WaypointMinimapIcon : MonoBehaviour
{
    public Transform target;                 // Alvo no mundo
    public RectTransform iconUI;             // Ícone do waypoint na UI
    public RectTransform minimapRect;        // RectTransform da imagem do minimapa (UI)
    public Transform player;                 // Referência ao jogador
    public float minimapWorldSize = 100f;    // Tamanho em unidades do mundo que o minimapa cobre (diâmetro)

    void Update()
    {
        Vector3 offset = target.position - player.position;
        Vector2 targetLocal = new Vector2(offset.x, offset.z);

        float halfSize = minimapWorldSize / 2f;
        float mapScale = minimapRect.rect.width / minimapWorldSize;

        Vector2 clampedLocal = targetLocal;

        // Verifica se está fora dos limites visíveis
        bool isOutside = Mathf.Abs(targetLocal.x) > halfSize || Mathf.Abs(targetLocal.y) > halfSize;

        if (isOutside)
        {
            // Limita a posição ao círculo do minimapa
            clampedLocal = targetLocal.normalized * halfSize;
        }

        // Converte para posição na UI
        Vector2 minimapPos = clampedLocal * mapScale;
        iconUI.anchoredPosition = minimapPos;

        // Ativa o ícone sempre (nunca desaparece)
        iconUI.gameObject.SetActive(true);
    }
}
