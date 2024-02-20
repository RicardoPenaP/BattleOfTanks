using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Gameplay.Input.Controls;

namespace Gameplay.Input
{
    [CreateAssetMenu(fileName = "NewInputReader", menuName = "Input/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event Action<Vector2> OnMoveInputDetected;
        public event Action<bool> OnPrimaryFireInputDetected;

        private Controls controls;

        public Vector2 MousePosition { get; private set; }

        private void OnEnable()
        {
            InitializeControls();

            controls.Player.Enable();

            MousePosition = Vector2.zero;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            HandleMoveInput(rawInput);
        }

        public void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                HandlePrimaryFireInput(true);
            }
            else if (context.canceled)
            {
                HandlePrimaryFireInput(false);
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            HandleMousePositionInput(rawInput);
        }

        private void InitializeControls()
        {
            if (controls != null)
            {
                return;
            }

            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        private void HandleMoveInput(Vector2 rawInput)
        {
            OnMoveInputDetected?.Invoke(rawInput);
        }

        private void HandlePrimaryFireInput(bool state)
        {
            OnPrimaryFireInputDetected?.Invoke(state);
        }

        private void HandleMousePositionInput(Vector2 rawInput)
        {
            MousePosition = rawInput;
        }
    }
}
