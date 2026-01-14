using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Death : MonoBehaviour
{
    [SerializeField] private UIDocument UI;
    [SerializeField] private Stats stats;

    private VisualElement main;

    void Awake()
    {
        main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => StartCoroutine(SceneUtil.LoadScene("Start")));
    }

    void FixedUpdate()
    {
        main.visible = stats.IsDeath;
    }
}