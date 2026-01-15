using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
public class AttackManager : MonoBehaviour
{
    public Transform AttackPoint;
    public float AttackRadius;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration;
    [SerializeField] private float knockbackForce = 1000f;
    public LayerMask EnemyLayer;
    private float lastAttackTime = -Mathf.Infinity;
    private bool CanAttack { get => !stats.IsDeath && Time.unscaledTime - lastAttackTime >= attackCooldown; }

    [SerializeField] private float maxCombo = 1;
    [SerializeField] private float comboDuration = 0;
    private int combo = 1;
    private IEnumerator comboResetCoroutine;

    private Rigidbody2D rb;
    private CombatAnimator animator;
    private MovementManager movement;
    private Stats stats;
    [SerializeField] private RegenerationManager regenerator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<CombatAnimator>();
        movement = GetComponentInParent<MovementManager>();
        stats = GetComponent<Stats>();
    }

    public void Attack()
    {
        if (!movement.IsGrounded || !CanAttack) return;

        if(comboResetCoroutine != null) StopCoroutine(comboResetCoroutine);

        animator.AnimateAttack(combo);
        combo++;
        if(combo > maxCombo) combo = 1;

        rb.linearVelocity = new(0, rb.linearVelocityY);

        lastAttackTime = Time.unscaledTime;

        movement.Disable();
        StartCoroutine(Util.Timeout(
            movement.Enable,
            attackDuration
        ));
        comboResetCoroutine = Util.Timeout(
            () => combo = 1,
            comboDuration
        );
        StartCoroutine(comboResetCoroutine);
    }

    public void Impact()
    {
        var opponents = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRadius, EnemyLayer);
        foreach (var opponent in opponents)
        {
            opponent.GetComponent<AttackManager>().TakeDamage(stats.Damage, movement.Direction, knockbackForce);
        }
    }

    public void TakeDamage(float damage, float direction, float knockback)
    {
        var shieldDamage = Mathf.Min(stats.Shield, damage);
        damage -= shieldDamage;
        stats.Shield -= shieldDamage;

        damage = Mathf.Min(damage, stats.Health);
        stats.Health -= damage;
        if(regenerator != null) regenerator.Disable();

        rb.AddForce(new Vector2(knockback * direction, 0), ForceMode2D.Force);

        animator.AnimateHurt();
        if (stats.IsDeath) animator.AnimateDeath();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
    }
}