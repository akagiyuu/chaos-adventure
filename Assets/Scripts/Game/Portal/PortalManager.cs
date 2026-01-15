using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private string nextLevel = "End";
    [SerializeField] private SceneManagerSO sceneManager;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerManager>())
            StartCoroutine(sceneManager.LoadScene(nextLevel));
    }
}
