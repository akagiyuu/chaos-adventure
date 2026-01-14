using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class StartMenu : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager;
    private UIDocument UI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Button playButton = UI.rootVisualElement.Query<Button>("play-button");
        playButton.clicked += () => StartCoroutine(sceneManager.LoadScene("Main"));

        Button exitButton = UI.rootVisualElement.Query<Button>("exit-button");
        exitButton.clicked += Application.Quit;
    }
}
