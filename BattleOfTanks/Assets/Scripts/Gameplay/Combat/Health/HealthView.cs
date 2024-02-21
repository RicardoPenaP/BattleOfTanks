using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Combat.Health
{
    public class HealthView : NetworkBehaviour
    {
        [Header("Health View")]
        [Header("View References")]
        [SerializeField] Image barImage;

        public void UpdateBarImage(float normalizedValue)
        {            
            normalizedValue = Mathf.Clamp(normalizedValue, 0, 1);
            barImage.fillAmount = normalizedValue;
        }
    }
}
