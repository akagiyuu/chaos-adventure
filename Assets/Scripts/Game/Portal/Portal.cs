using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextLevel = "End";
    [SerializeField] private SceneManagerSO sceneManager;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
            StartCoroutine(sceneManager.LoadScene(nextLevel));
    }
}
