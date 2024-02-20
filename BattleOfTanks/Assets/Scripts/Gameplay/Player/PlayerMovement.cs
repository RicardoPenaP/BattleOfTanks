using Gameplay.Input;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Player Movement")]

        [Header("Input Reference")]
        [SerializeField] private InputReader inputReader;

        [Header("Tank Parts References")]
        [SerializeField] private Transform tankBodyTransform;
        [SerializeField] private new Rigidbody2D rigidbody2D;

        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 4f;
        [SerializeField] private float turningRate = 30f;

        private Vector2 previusMovementInput;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }

            inputReader.OnMoveInputDetected += InputReader_OnMoveInputDetected;
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            Rotate();
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            Move();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }

            inputReader.OnMoveInputDetected -= InputReader_OnMoveInputDetected;
        }

        private void InputReader_OnMoveInputDetected(Vector2 rawInput)
        {
            HandleMovementInput(rawInput);
        }

        private void HandleMovementInput(Vector2 rawInput)
        {
            previusMovementInput = rawInput;
        }

        private void Move()
        {
            rigidbody2D.velocity = (Vector2)tankBodyTransform.up * previusMovementInput.y * movementSpeed;
        }

        private void Rotate()
        {
            float zRotation = previusMovementInput.x * -turningRate * Time.deltaTime;
            tankBodyTransform.Rotate(0f, 0f, zRotation);
        }
    }
}
