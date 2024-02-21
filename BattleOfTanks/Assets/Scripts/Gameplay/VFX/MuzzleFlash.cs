using System.Collections;
using UnityEngine;

namespace Gameplay.Player.VFX
{
    public class MuzzleFlash : MonoBehaviour
    {
        [Header("Muzzle Flash")]

        [Header("Game Objects references")]
        [SerializeField] private GameObject spriteRenderer;

        [Header("Settings")]
        [SerializeField] private float activeTime = .2f;

        float timer = 0;
        bool isActivated = false;

        public void Activate()
        {
            timer = 0;

            if (isActivated)
            {
                return;
            }

            isActivated = true;
            spriteRenderer.SetActive(true);
            StartCoroutine(ActivationRoutine());
        }

        private IEnumerator ActivationRoutine()
        {
            while (timer < activeTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            isActivated = false;
            spriteRenderer.SetActive(false);
        }
    }
}
