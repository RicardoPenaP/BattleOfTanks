using Gameplay.Input;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    public class PlayerAiming : NetworkBehaviour
    {
        [Header("Player Aiming")]
        [Header("Input Reference")]
        [SerializeField] private InputReader inputReader;

        [Header("Tank Parts References")]
        [SerializeField] private Transform tankTurretTransform;

        private void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(inputReader.MousePosition);

            Vector2 aimDirection = (worldMousePosition - (Vector2)transform.position).normalized;

            tankTurretTransform.up = aimDirection;
        }
    }
}
