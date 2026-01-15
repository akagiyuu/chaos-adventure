using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class StartMenu : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager;
    [SerializeField] private TimerSO timer;
    private UIDocument UI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Image playButton = UI.rootVisualElement.Query<Image>();
        playButton.RegisterCallback<PointerDownEvent>(e =>
        {
            StartCoroutine(sceneManager.LoadLevel(1));
            timer.Reset();
        });
    }
}