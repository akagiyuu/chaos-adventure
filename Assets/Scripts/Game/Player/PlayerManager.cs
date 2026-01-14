using UnityEngine;

[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(AttackManager))]
public class PlayerManager : MonoBehaviour
{
    private MovementManager movement;
    private AttackManager combat;
    [SerializeField] InputManagerSO input;

    void Awake()
    {
        movement = GetComponent<MovementManager>();
        combat = GetComponent<AttackManager>();
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