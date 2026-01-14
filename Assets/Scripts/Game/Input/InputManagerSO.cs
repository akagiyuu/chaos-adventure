using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "Scriptable Objects/InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    public Vector2 Move { get; private set; }
    public bool IsAttack { get; private set; }
    public Action TogglePauseEvent;

    private Controls controls;

    private void OnEnable()
    {
        controls ??= new Controls();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

        controls.Player.Attack.performed += OnAttackPerformed;
        controls.Player.Attack.canceled += OnAttackCanceled;

        controls.UI.TogglePause.performed += OnTogglePausePerformed;

        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls == null) return;

        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;

        controls.Player.Attack.performed -= OnAttackPerformed;
        controls.Player.Attack.canceled -= OnMoveCanceled;

        controls.UI.TogglePause.performed -= OnTogglePausePerformed;

        controls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => Move = ctx.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext _) => Move = Vector2.zero;

    private void OnAttackPerformed(InputAction.CallbackContext ctx) => IsAttack = ctx.ReadValueAsButton();
    private void OnAttackCanceled(InputAction.CallbackContext _) => IsAttack = false;

    private void OnTogglePausePerformed(InputAction.CallbackContext _) => TogglePauseEvent?.Invoke();
}