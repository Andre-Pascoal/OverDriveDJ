using UnityEngine;

public class LeaveGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("🚪 A sair do jogo...");
        Application.Quit();
    }
}
