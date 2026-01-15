using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
public class MovementManager : MonoBehaviour
{
    [SerializeField] private float moveForce = 80f;
    [SerializeField] private float acceleration = 80f;

    [SerializeField] private float jumpForce = 35f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 groundCheckOffset = new(0, -0.5f);

    [SerializeField] private float minY;

    private Rigidbody2D rb;
    private Stats stats;

    private bool canMove = true;
    public float Direction { get; private set; } = 1f;
    private bool canJump = true;
    public bool IsGrounded { get; private set; } = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
        IsGrounded = CheckGround();
        if(IsFalling()) stats.Health = 0;
    }

    public void Move(Vector2 input)
    {
        if (!canMove) return;

        if (input.x * Direction < 0)
            Flip();

        var velocityX = input.x * moveForce;
        velocityX = Mathf.MoveTowards(rb.linearVelocityX, velocityX, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocityY);
    }

    public void Jump()
    {
        IsGrounded = CheckGround();
        if (!canJump || !IsGrounded) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        IsGrounded = false;
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
    }

    private bool CheckGround()
    {
        Vector2 groundCheckPos = (Vector2)transform.position + groundCheckOffset;
        return Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
    }

    private bool IsFalling()
    {
        return transform.position.y < minY;
    }

    private void Flip()
    {
        Direction = -Direction;
        transform.Rotate(0, 180, 0);
    }

    public void Disable()
    {
        canMove = false;
        canJump = false;
    }

    public void Enable()
    {
        canMove = true;
        canJump = true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 groundCheckPos = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckRadius);
    }
}