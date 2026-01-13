using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIDocument UI;

    void Start()
    {
        VisualElement pauseMenu = UI.rootVisualElement.Query<VisualElement>("pause-menu");
        PauseController.Instance.OnChanged += isPaused => pauseMenu.visible = isPaused;

        Button resumeButton = UI.rootVisualElement.Query<Button>("resume-button");
        resumeButton.clicked += () => PauseController.Instance.Resume();

        Button exitButton = UI.rootVisualElement.Query<Button>("exit-button");
        exitButton.clicked += Application.Quit;
    }
}
