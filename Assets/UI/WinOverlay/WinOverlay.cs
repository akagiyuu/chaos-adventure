using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class WinOverlay : MonoBehaviour
{
    [SerializeField] private TimerSO timer;
    [SerializeField] private SceneManagerSO sceneManager;
    private UIDocument UI;

    private VisualElement main;
    private Label title;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => StartCoroutine(sceneManager.LoadStart()));

        title = UI.rootVisualElement.Query<Label>("title");
    }

    public void Display() {
        title.text = $"You completed all level in {Mathf.RoundToInt(timer.Elapsed())} seconds";
        main.visible = true;
    }
}