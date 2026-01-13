using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UIDocument ui;
    private VisualElement pauseMenu;

    public static PauseController Instance;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        pauseMenu = ui.rootVisualElement.Query("pause-menu");

        Button resumeButton = ui.rootVisualElement.Query<Button>("resume");
        resumeButton.clicked += Resume;

        Button exitButton = ui.rootVisualElement.Query<Button>("exit");
        exitButton.clicked += Application.Quit;
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.visible = isPaused;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.visible = isPaused;
        Time.timeScale = 1f;
    }

    public void Toggle()
    {
        if (isPaused) Resume();
        else Pause();
    }
}
