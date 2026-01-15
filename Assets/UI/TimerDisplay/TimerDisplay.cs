using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TimerSO timer;
    [SerializeField] private Label timerLabel;

    private UIDocument UI;

    void Awake()
    {
        UI = GetComponent<UIDocument>();

        timerLabel = UI.rootVisualElement.Query<Label>("timer");
    }

    void FixedUpdate() => timerLabel.text = $"Time: {Mathf.FloorToInt(timer.Elapsed())}";
}
