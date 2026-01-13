using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private UIDocument UI;

    void Awake()
    {
        Button playButton = UI.rootVisualElement.Query<Button>("play-button");
        playButton.clicked += () => SceneManager.LoadScene("Main");

        Button exitButton = UI.rootVisualElement.Query<Button>("exit-button");
        exitButton.clicked += Application.Quit;
    }
}
