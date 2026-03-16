using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Toast : MonoBehaviour
{
    private UIDocument UI;
    private VisualElement main;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => main.visible = false);
    }

    public void Notify(string Title, string Detail)
    {
        Label titleElement = UI.rootVisualElement.Query<Label>("title");
        titleElement.text = Title;

        Label detailElement = UI.rootVisualElement.Query<Label>("detail");
        detailElement.text = Detail;

        main.visible = true;
    }
}
