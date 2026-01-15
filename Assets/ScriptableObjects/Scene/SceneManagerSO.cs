using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneManagerSO", menuName = "Scriptable Objects/SceneManagerSO")]
public class SceneManagerSO : ScriptableObject
{
    [SerializeField] private GameObject overlayPrefab;
    [SerializeField] private string startScene = "Start";
    [SerializeField] private string loseScene = "Lose";
    [SerializeField] private string winScene = "Win";
    [SerializeField] private string levelPrefix = "Level";

    public IEnumerator LoadStart() => LoadScene(startScene);
    public IEnumerator LoadLose() => LoadScene(loseScene);
    public IEnumerator LoadWin() => LoadScene(winScene);
    public IEnumerator LoadLevel(int level) => LoadScene($"{levelPrefix} {level}");

    private IEnumerator LoadScene(string sceneName)
    {
        var overlayObject = Instantiate(overlayPrefab);
        var overlay = overlayObject.GetComponent<SceneTransitionOveray>();

        yield return overlay.FadeOut();

        SceneManager.LoadScene(sceneName);

        yield return overlay.FadeIn();

        Destroy(overlayObject);
    }
}
