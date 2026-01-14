using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneManagerSO", menuName = "Scriptable Objects/SceneManagerSO")]
public class SceneManagerSO : ScriptableObject
{
    [SerializeField] private GameObject overlayPrefab;

    public IEnumerator LoadScene(string sceneName)
    {
        var overlayObject = Instantiate(overlayPrefab);
        var overlay = overlayObject.GetComponent<SceneTransitionOveray>();

        yield return overlay.FadeOut();

        SceneManager.LoadScene(sceneName);

        yield return overlay.FadeIn();

        Destroy(overlayObject);
    }
}
