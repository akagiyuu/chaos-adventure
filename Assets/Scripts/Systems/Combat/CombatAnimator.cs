using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CombatAnimator : MonoBehaviour
{
    private Animator animator;
    private Movement movement;
    private AttackController combat;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<Movement>();
        combat = GetComponentInParent<AttackController>();
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

    public void AnimateDeath()
    {
        animator.SetBool("isDeath", true);
    }
}