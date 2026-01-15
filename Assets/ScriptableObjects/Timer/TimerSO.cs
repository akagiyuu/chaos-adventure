using UnityEngine;

[CreateAssetMenu(fileName = "TimerSO", menuName = "Scriptable Objects/TimerSO")]
public class TimerSO : ScriptableObject
{
    private float start = 0;
    public float Elapsed { get => Time.fixedTime - start; }

    void Awake() => start = Time.fixedTime;

    public void Reset() => start = Time.fixedTime;
}
