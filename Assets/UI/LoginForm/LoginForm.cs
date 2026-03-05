using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LoginForm : MonoBehaviour
{
    private readonly CancellationTokenSource cts = new();

    private UIDocument UI;
    [SerializeField]
    private UIDocument registerUI;
    [SerializeField]
    private UIDocument recordUI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Button loginButton = UI.rootVisualElement.Query<Button>("login");
        loginButton.clicked += Login;

        Button gotoRegisterButton = UI.rootVisualElement.Query<Button>("register");
        gotoRegisterButton.clicked += GotoRegister;
    }

    private async void Login()
    {
        var body = new ApiClient.LoginData
        {
            username = GetUsername(),
            password = GetPassword(),
        };

        try
        {
            var token = await ApiClient.LoginAsync(body, cts.Token);
            Debug.Log(token);
        }
        catch (ApiClient.ApiException ex)
        {
            Debug.LogError($"API error ({ex.StatusCode}): {ex.Message}");
            return;
        }

        Hide();

        VisualElement recordMain = recordUI.rootVisualElement.Query<VisualElement>("main");
        recordMain.visible = true;
    }

    private void GotoRegister()
    {
        Hide();

        VisualElement registerMain = registerUI.rootVisualElement.Query<VisualElement>("main");
        registerMain.visible = true;
    }

    private void Hide()
    {
        VisualElement main = UI.rootVisualElement.Query<VisualElement>("main");
        main.visible = false;
    }

    private string GetUsername()
    {
        TextField usernameInput = UI.rootVisualElement.Query<TextField>("username");
        return usernameInput.value;
    }

    private string GetPassword()
    {
        TextField passwordInput = UI.rootVisualElement.Query<TextField>("password");
        return passwordInput.value;
    }
}
