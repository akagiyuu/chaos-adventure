using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public bool IsPaused { get; private set; } = false;
    public Action<bool> OnChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Pause()
    {
        IsPaused = true;
        OnChanged(IsPaused);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        IsPaused = false;
        OnChanged(IsPaused);
        Time.timeScale = 1f;
    }

    public void Toggle()
    {
        if (IsPaused) Resume();
        else Pause();
    }
}
