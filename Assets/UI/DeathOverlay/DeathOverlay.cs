using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DeathOverlay : MonoBehaviour
{
    [SerializeField] private SceneManagerSO sceneManager;
    private UIDocument UI;

    private VisualElement main;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        main = UI.rootVisualElement.Query<VisualElement>();
        main.RegisterCallback<PointerDownEvent>(e => StartCoroutine(sceneManager.LoadStart()));
    }
}