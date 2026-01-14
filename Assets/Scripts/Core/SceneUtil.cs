using System.Collections;
using UnityEngine.SceneManagement;

public static class SceneUtil
{
    private const float delay = 0.5f;

    public static IEnumerator LoadScene(string name)
    {
        return Util.Timeout(() => SceneManager.LoadScene(name), delay);
    }
}
