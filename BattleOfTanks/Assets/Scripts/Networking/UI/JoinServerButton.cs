using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace Networking.UI
{
    public class JoinServerButton : MonoBehaviour
    {
        private Button joinButton;

        private void Awake()
        {
            joinButton = GetComponent<Button>();
        }

        private void Start()
        {
            joinButton.onClick.AddListener(Join);
        }

        private void OnDestroy()
        {
            joinButton.onClick.RemoveListener(Join);
        }

        public void Join()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
