using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private int nextLevel;
    [SerializeField] private SceneManagerSO sceneManager;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>())
            StartCoroutine(sceneManager.LoadLevel(nextLevel));
    }
}
