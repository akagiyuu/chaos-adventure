using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Death : MonoBehaviour
{
    [SerializeField] private UIDocument UI;

    void Awake()
    {
        VisualElement main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => SceneManager.LoadScene("Start"));
    }
}