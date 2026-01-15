using UnityEngine;

[CreateAssetMenu(fileName = "TimerSO", menuName = "Scriptable Objects/TimerSO")]
public class TimerSO : ScriptableObject
{
    private float start = 0;

    void Awake() => start = Time.fixedTime;

    public float Elapsed() => Time.fixedTime - start;

    public void Reset() => start = Time.fixedTime;
}
