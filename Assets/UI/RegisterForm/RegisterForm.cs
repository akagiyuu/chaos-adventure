using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RegisterForm : MonoBehaviour
{
    private readonly CancellationTokenSource cts = new();

    private UIDocument UI;
    [SerializeField]
    private UIDocument loginUI;
    [SerializeField]
    private UIDocument recordUI;
    [SerializeField]
    private Toast toast;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        Button registerButton = UI.rootVisualElement.Query<Button>("register");
        registerButton.clicked += Register;

        Button gotoLoginButton = UI.rootVisualElement.Query<Button>("login");
        gotoLoginButton.clicked += GotoLogin;
    }

    private async void Register()
    {
        var username = GetUsername();
        var password = GetPassword();
        var confirmPassword = GetConfirmPassword();
        if (password != confirmPassword)
        {
            toast.Notify("Register Error", "Password does not match");
            Debug.Log("Password does not match");
            return;
        }

        var body = new ApiClient.RegisterData
        {
            username = username,
            password = password,
        };

        try
        {
            var token = await ApiClient.RegisterAsync(body, cts.Token);
            Debug.Log(token);
        }
        catch (ApiClient.ApiException ex)
        {
            toast.Notify("Register Error", ex.Error.detail);
            Debug.LogError($"API error ({ex.StatusCode}): {ex.Error.detail}");
            return;
        }

        Hide();

        VisualElement recordMain = recordUI.rootVisualElement.Query<VisualElement>("main");
        recordMain.visible = true;
    }

    private void GotoLogin()
    {
        Hide();

        VisualElement loginMain = loginUI.rootVisualElement.Query<VisualElement>("main");
        loginMain.visible = true;
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

    private string GetConfirmPassword()
    {
        TextField confirmPasswordInput = UI.rootVisualElement.Query<TextField>("confirm-password");
        return confirmPasswordInput.value;
    }
}
