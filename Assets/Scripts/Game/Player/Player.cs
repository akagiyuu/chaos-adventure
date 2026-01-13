using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AttackController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private AttackController combat;

    void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<AttackController>();
    }

    private void FixedUpdate()
    {
        Vector2 input = InputController.Instance.MoveInput;

        movement.Move(input);
        if (input.y > 0.5f)
            movement.Jump();
        if (InputController.Instance.AttackInput)
            combat.Attack();
    }
}