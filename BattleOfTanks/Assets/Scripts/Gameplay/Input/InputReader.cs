using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Gameplay.Input.Controls;

namespace Gameplay.Input
{
    [CreateAssetMenu(fileName = "NewInputReader", menuName = "Input/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event Action<Vector2> MoveEvent;
        public event Action<bool> PrimaryFireEvent;

        private Controls controls;

        private void OnEnable()
        {
            InitializeControls();

            controls.Player.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            MoveEvent?.Invoke(rawInput);
        }

        public void OnPrimaryFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PrimaryFireEvent?.Invoke(true);
            }
            else if(context.canceled)
            {
                PrimaryFireEvent?.Invoke(false);
            }
            
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
    }
}
