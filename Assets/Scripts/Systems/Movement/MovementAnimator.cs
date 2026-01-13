using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovementAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Movement movement;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<Movement>();
    }

    private void FixedUpdate()
    {
        bool isGrounded = movement.IsGrounded;

        animator.SetFloat("xVelocity", rb.linearVelocityX);
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetBool("isGrounded", isGrounded);
    }
}