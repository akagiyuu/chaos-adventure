using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PauseManagerSO pauseManager;
    private UIDocument UI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        VisualElement main = UI.rootVisualElement.Query<VisualElement>("pause-menu");
        pauseManager.ChangeEvent += (isPaused) => main.visible = isPaused;

        Button resumeButton = UI.rootVisualElement.Query<Button>("resume-button");
        resumeButton.clicked += () => pauseManager.Resume();

        Button exitButton = UI.rootVisualElement.Query<Button>("exit-button");
        exitButton.clicked += Application.Quit;
    }
}
