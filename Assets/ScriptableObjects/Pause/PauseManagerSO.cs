using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PauseManagerSO", menuName = "Scriptable Objects/PauseManagerSO")]
public class PauseManagerSO : ScriptableObject
{
    [SerializeField] private InputManagerSO input;
    private bool isPaused = false;
    public Action<bool> ChangeEvent;

    void OnEnable()
    {
        input.TogglePauseEvent += Toggle;
    }

    void OnDisable()
    {
        input.TogglePauseEvent -= Toggle;
    }

    public void Pause()
    {
        isPaused = true;
        ChangeEvent?.Invoke(isPaused);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        ChangeEvent?.Invoke(isPaused);
        Time.timeScale = 1f;
    }

    public void Toggle()
    {
        if (isPaused) Resume();
        else Pause();
    }
}
