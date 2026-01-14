using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AttackController))]
public class Player : MonoBehaviour
{
    private Movement movement;
    private AttackController combat;
    [SerializeField] InputManagerSO input;

    void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<AttackController>();
    }

    void FixedUpdate()
    {
        movement.Move(input.Move);
        if (input.Move.y > 0.5f)
            movement.Jump();
        if (input.IsAttack)
            combat.Attack();
    }
}