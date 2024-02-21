using Gameplay.Combat.Health;
using UnityEngine;
using Unity.Netcode;

namespace Gameplay.Combat.DamageDealers
{
    public class DealDamageOnContact : MonoBehaviour
    {
        [Header("Deal Damage On Contact")]

        [Header("Settings")]
        [SerializeField] private int damage;

        private ulong ownerClientId;

        public void SetOwnerClientId(ulong ownerClientId)
        {
            this.ownerClientId = ownerClientId;
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.attachedRigidbody == null)
            {
                return;
            }

            if (collider2D.TryGetComponent(out NetworkObject networkObject))
            {
                if (networkObject.OwnerClientId == ownerClientId)
                {
                    return;
                }
            }

            if (collider2D.TryGetComponent(out HealthController healthController))
            {
                healthController.TakeDamage(damage);
            }
        }
    }
}
