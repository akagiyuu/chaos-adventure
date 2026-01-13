using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    private Controls controls;
    public Vector2 MoveInput { get; private set; }
    public bool AttackInput { get; private set; }


    private void Awake()
    {
        if (Instance == null) Instance = this;

        controls = new Controls();

        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        controls.Player.Attack.performed += ctx => AttackInput = ctx.ReadValueAsButton();
        controls.Player.Attack.canceled += ctx => AttackInput = false;

        controls.UI.Pause.performed += ctx => PauseController.Instance.Toggle();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}