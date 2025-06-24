using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        // Parar a música do menu
        if (BackgroundMusicManager.instance != null)
            BackgroundMusicManager.instance.ToggleMusic(false);

        // Carregar a próxima cena (ex: cutscene)
        SceneManager.LoadScene(sceneToLoad);
    }
}
