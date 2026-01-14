using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CombatAnimator : MonoBehaviour
{
    private Animator animator;
    private MovementManager movement;
    private AttackManager combat;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<MovementManager>();
        combat = GetComponentInParent<AttackManager>();
    }

    public void AnimateAttack()
    {
        animator.SetTrigger("attack");
    }

    private void OnImpact()
    {
        combat.Impact();
    }

    public void AnimateHurt()
    {
        animator.SetTrigger("hurt");
    }

    private void OnHurt()
    {
        movement.Disable();
    }

    private void OnHurtEnd()
    {
        movement.Enable();
    }

    private void OnDeathEnd()
    {
        movement.Disable();
        StartCoroutine(Util.Timeout(
            () => Destroy(combat.gameObject),
            1f
        ));
    }

    public void AnimateDeath()
    {
        animator.SetBool("isDeath", true);
    }
}