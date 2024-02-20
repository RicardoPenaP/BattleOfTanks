using Unity.Netcode;
using UnityEngine;

namespace Networking.UI
{
    public class ConectionButtons : MonoBehaviour
    {
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        public void StarHost()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
