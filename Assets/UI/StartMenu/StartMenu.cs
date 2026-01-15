using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class StartMenu : MonoBehaviour
{
    [SerializeField] private string firstLevel = "Level 1";
    [SerializeField] private SceneManagerSO sceneManager;
    private UIDocument UI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Image playButton = UI.rootVisualElement.Query<Image>();
        playButton.RegisterCallback<PointerDownEvent>(e => StartCoroutine(sceneManager.LoadScene(firstLevel)));
    }
}