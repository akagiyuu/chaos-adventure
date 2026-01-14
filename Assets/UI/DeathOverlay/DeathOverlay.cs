using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DeathOverlay : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager;
    [SerializeField] private Stats stats;
    private UIDocument UI;

    private VisualElement main;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => StartCoroutine(sceneManager.LoadScene("Start")));
    }

    void FixedUpdate()
    {
        main.visible = stats.IsDeath;
    }
}