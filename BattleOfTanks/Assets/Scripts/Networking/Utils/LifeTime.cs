using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Utils
{
    public class LifeTime : MonoBehaviour
    {
        [Header("Life Time")]
        [SerializeField] private float lifeTime = 2f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
