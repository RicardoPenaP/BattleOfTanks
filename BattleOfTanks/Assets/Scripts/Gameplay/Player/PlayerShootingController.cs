using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Gameplay.Input;

namespace Gameplay
{
    public class PlayerShootingController : NetworkBehaviour
    {
        [Header("Player Shooting Controller")]

        [Header("Input References")]
        [SerializeField] private InputReader inputReader;

        [Header("Prefabs References")]
        [SerializeField] private GameObject serverProjectilePrefab;
        [SerializeField] private GameObject clientProjectilePrefab;

        [Header("Tank References")]
        [SerializeField] private Transform[] canonShootingPointsTransforms;

        [Header("Settings")]
        [SerializeField] private float projectileSpeed;

        private int shootingPointsTranformsIndex;
        private bool shouldFire = true;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }

            Initializalitation();
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (!shouldFire)
            {
                return;
            }

            Shoot();
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }

            DesInitializalitation();
        }

        private void Initializalitation()
        {
            inputReader.OnPrimaryFireInputDetected += InputReader_OnPrimaryFireInputDetected;
            shootingPointsTranformsIndex = Random.Range(0, canonShootingPointsTransforms.Length);
        }

        private void DesInitializalitation()
        {
            inputReader.OnPrimaryFireInputDetected -= InputReader_OnPrimaryFireInputDetected;
        }

        private void InputReader_OnPrimaryFireInputDetected(bool state)
        {
            shouldFire = state;
        }

        private void Shoot()
        {
            SpawnProjectileServerRpc(canonShootingPointsTransforms[shootingPointsTranformsIndex].position,
                                  canonShootingPointsTransforms[shootingPointsTranformsIndex].up);

            SpawnClientProjectile(canonShootingPointsTransforms[shootingPointsTranformsIndex].position,
                                  canonShootingPointsTransforms[shootingPointsTranformsIndex].up);
        }

        private void SpawnClientProjectile(Vector3 spawPosition, Vector3 direction)
        {
            GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawPosition, Quaternion.identity);
            projectileInstance.transform.up = direction;
        }

        [ServerRpc]
        private void SpawnProjectileServerRpc(Vector3 spawPosition, Vector3 direction)
        {
            GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawPosition, Quaternion.identity);
            projectileInstance.transform.up = direction;
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc(Vector3 spawPosition, Vector3 direction)
        {
            if (IsOwner)
            {
                return;
            }
            SpawnClientProjectile(spawPosition, direction);
        }
    }
}
