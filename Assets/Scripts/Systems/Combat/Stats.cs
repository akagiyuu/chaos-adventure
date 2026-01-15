using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float MaxHealth = 100f;
    [HideInInspector] public float Health;
    [SerializeField] private Transform healthBar;

    public float MaxShield = 10f;
    [HideInInspector] public float Shield;
    [SerializeField] private Transform shieldBar;

    [SerializeField] private float lerpSpeed = 3f;

    public float Damage = 10f;

    public bool IsDeath
    {
        get => Health <= 0;
    }

    void Awake()
    {
        Health = MaxHealth;
        Shield = MaxShield;
    }

    void FixedUpdate()
    {
        if (healthBar != null)
        {
            var healthPercent = GetSmoothPercent(healthBar.localScale.x, Health / MaxHealth);
            healthBar.localScale = new Vector3(healthPercent, healthBar.localScale.y, healthBar.localScale.z);
        }

        if (shieldBar != null)
        {
            var shieldPercent = GetSmoothPercent(shieldBar.localScale.x, Shield / MaxShield);
            shieldBar.localScale = new Vector3(shieldPercent, shieldBar.localScale.y, shieldBar.localScale.z);
        }
    }

    private float GetSmoothPercent(float previous, float current)
    {
        return Mathf.Lerp(previous, current, lerpSpeed * Time.fixedDeltaTime);
    }
}
