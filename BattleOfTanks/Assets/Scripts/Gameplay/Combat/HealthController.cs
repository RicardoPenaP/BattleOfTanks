using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
    public class HealthController : NetworkBehaviour
    {
        [Header("Health Controller")]
        [SerializeField] private int maxHealth = 100;

        public event Action<HealthController> OnDie;

        public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
        public int MaxHealth => maxHealth;

        private bool isDead = false;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }

            CurrentHealth.Value = maxHealth;
        }

        public void TakeDamage(int value)
        {
            SetCurrentHealth(-Mathf.Abs(value));
        }

        public void RestoreHealth(int value)
        {
            SetCurrentHealth(Mathf.Abs(value));
        }

        private void SetCurrentHealth(int value)
        {
            if (isDead)
            {
                return;
            }

            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value + value, 0, maxHealth);

            if (CurrentHealth.Value == 0)
            {
                isDead = true;
                OnDie?.Invoke(this);
            }
        }
    }
}
