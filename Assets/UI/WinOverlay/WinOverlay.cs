using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class WinOverlay : MonoBehaviour
{
    [SerializeField] private TimerSO timer;
    [SerializeField] private SceneManagerSO sceneManager;
    private UIDocument UI;

    private readonly CancellationTokenSource cts = new();
    private VisualElement main;
    private Label title;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        main = UI.rootVisualElement.Query<VisualElement>("main");
        main.RegisterCallback<PointerDownEvent>(e => StartCoroutine(sceneManager.LoadStart()));

        title = UI.rootVisualElement.Query<Label>("title");
    }

    public async void Display()
    {
        var time = timer.Elapsed();
        title.text = $"You completed all level in {Mathf.RoundToInt(time)} seconds";
        main.visible = true;

        var data = new ApiClient.CreateRecordData
        {
            time = time
        };

        try
        {
            await ApiClient.CreateRecordAsync(data, cts.Token);
        }
        catch (ApiClient.ApiException ex)
        {
            Debug.LogError($"API error ({ex.StatusCode}): {ex.Message}");
            return;
        }
    }
}