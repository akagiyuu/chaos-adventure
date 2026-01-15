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

        VisualElement main = UI.rootVisualElement.Query<VisualElement>("main");
        pauseManager.ChangeEvent += (isPaused) => main.visible = isPaused;

        VisualElement resumeButton = UI.rootVisualElement.Query<VisualElement>("resume-button");
        resumeButton.RegisterCallback<PointerDownEvent>(e => pauseManager.Resume());

        VisualElement exitButton = UI.rootVisualElement.Query<VisualElement>("exit-button");
        resumeButton.RegisterCallback<PointerDownEvent>(e => Application.Quit());
    }
}
