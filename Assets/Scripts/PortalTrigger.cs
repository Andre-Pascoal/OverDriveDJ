using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "PortalLevel";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bus"))
        {
            SceneTransition.Instance.FadeToScene(sceneToLoad);
        }
    }
}
