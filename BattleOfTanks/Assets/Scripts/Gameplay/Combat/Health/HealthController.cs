using System;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Combat.Health
{
    public class HealthController : NetworkBehaviour
    {
        [Header("Health Controller")]

        [Header("Controller References")]
        [SerializeField] private HealthView healthView;

        [Header("Settings")]
        [SerializeField] private int maxHealth = 100;

        public event Action<HealthController> OnDie;

        public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
        public int MaxHealth => maxHealth;

        private bool isDead = false;

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                CurrentHealth.OnValueChanged += CurrentHealth_OnValueChanged;
            }

            if (!IsServer)
            {
                return;
            }

            SetCurrentHealth(maxHealth);
        }       

        public override void OnNetworkDespawn()
        {
            if (IsClient)
            {
                CurrentHealth.OnValueChanged -= CurrentHealth_OnValueChanged;
            }
        }

        private void CurrentHealth_OnValueChanged(int previousValue, int newValue)
        {
            float normalizedHealth = (float)newValue / maxHealth;

            healthView.UpdateBarImage(normalizedHealth);
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

            float normalizedHealth = (float)CurrentHealth.Value / maxHealth;

            healthView.UpdateBarImage(normalizedHealth);

            if (CurrentHealth.Value == 0)
            {
                isDead = true;
                OnDie?.Invoke(this);
            }
        }

        
    }
}
