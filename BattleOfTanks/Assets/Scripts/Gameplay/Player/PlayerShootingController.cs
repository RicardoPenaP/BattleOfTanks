using Gameplay.Combat.DamageDealers;
using Gameplay.Input;
using Gameplay.Player.VFX;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Player
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
        [SerializeField] private Transform[] gunShootingPointsTransforms;
        [SerializeField] private Collider2D tankCollider;

        [Header("VFX References")]
        [SerializeField] private MuzzleFlash[] gunVFXGameObjects;

        [Header("Settings")]
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float fireRate;

        private int shootingPointsTranformsIndex;
        private bool shouldFire = true;
        private bool canShoot = true;

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
            shootingPointsTranformsIndex = Random.Range(0, gunShootingPointsTransforms.Length);
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
            if (!canShoot)
            {
                return;
            }

            SpawnProjectileServerRpc(gunShootingPointsTransforms[shootingPointsTranformsIndex].position,
                                  gunShootingPointsTransforms[shootingPointsTranformsIndex].up, shootingPointsTranformsIndex);

            SpawnClientProjectile(gunShootingPointsTransforms[shootingPointsTranformsIndex].position,
                                  gunShootingPointsTransforms[shootingPointsTranformsIndex].up, shootingPointsTranformsIndex);


            shootingPointsTranformsIndex = shootingPointsTranformsIndex < gunShootingPointsTransforms.Length - 1 ?
                                            shootingPointsTranformsIndex + 1 : 0;

            canShoot = false;
            StartCoroutine(ShootCooldownRoutine());
        }

        private void SpawnClientProjectile(Vector3 spawnPosition, Vector3 direction, int index)
        {
            ProjectileInstantiation(clientProjectilePrefab, spawnPosition, direction);
            ActivateVFX(index);
        }

        [ServerRpc]
        private void SpawnProjectileServerRpc(Vector3 spawnPosition, Vector3 direction, int index)
        {
            ProjectileInstantiation(serverProjectilePrefab, spawnPosition, direction);
            SpawnProjectileClientRpc(spawnPosition, direction, index);
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc(Vector3 spawnPosition, Vector3 direction, int index)
        {
            if (IsOwner)
            {
                return;
            }
            SpawnClientProjectile(spawnPosition, direction, index);
        }

        private void ProjectileInstantiation(GameObject projectilePrefab, Vector3 spawnPosition, Vector3 direction)
        {
            GameObject projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectileInstance.transform.up = direction;

            Physics2D.IgnoreCollision(tankCollider, projectileInstance.GetComponent<Collider2D>());

            if (projectileInstance.TryGetComponent(out Rigidbody2D projectileRigidbody2D))
            {
                projectileRigidbody2D.velocity = projectileRigidbody2D.transform.up * projectileSpeed;
            }

            if (projectileInstance.TryGetComponent(out DealDamageOnContact dealDamageOnContact))
            {
                dealDamageOnContact.SetOwnerClientId(OwnerClientId);
            }
        }

        private void ActivateVFX(int index)
        {
            gunVFXGameObjects[index].Activate();
        }

        private IEnumerator ShootCooldownRoutine()
        {
            float timer = 1 / fireRate;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            canShoot = true;
        }
    }
}
