using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Regenerator : MonoBehaviour
{
    [SerializeField] private float healthRegenPercent = 0.1f;
    [SerializeField] private float healthRegenDelay = 10;
    [SerializeField] private float shieldRegenPercent = 0.1f;
    [SerializeField] private float shieldRegenDelay = 2;

    private Stats stats;

    private float lastDisable = -Mathf.Infinity;

    void Awake()
    {
        stats = GetComponent<Stats>();

        StartCoroutine(Util.Periodic(() =>
        {
            if (Time.unscaledTime - lastDisable < shieldRegenDelay) return;
            if (stats.Shield == stats.MaxShield) return;

            stats.Shield = Mathf.Min(
                stats.Shield + Mathf.Floor(stats.MaxShield * shieldRegenPercent),
                stats.MaxShield
            );
        }, 1f));

        StartCoroutine(Util.Periodic(() =>
        {
            if (Time.unscaledTime - lastDisable < healthRegenDelay) return;
            if(stats.Shield == 0) return;
            if (stats.Health == stats.MaxHealth) return;

            stats.Health = Mathf.Min(
                stats.Health + Mathf.Floor(stats.MaxHealth * healthRegenPercent),
                stats.MaxHealth
            );
        }, 1f));
    }

    public void Disable()
    {
        lastDisable = Time.unscaledTime;
    }
}
