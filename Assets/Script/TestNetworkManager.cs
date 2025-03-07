using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    //[SerializeField] private GameObject asdasdasd;
  //  public GameObject AAAAAAAAAAAAAAA;

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }


    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;

        Transform spawnPoint = PlayerSpawner.Instance.GetNextSpawnPoint();

       // GameObject playerPrefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;
        GameObject playerInstance = Instantiate(PlayerSpawner.Instance.playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Spawn the player and assign ownership to the client
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        Debug.Log(clientId);
    }
}

//using UnityEngine;
//using Unity.Netcode;

//public class CustomNetworkManager : NetworkBehaviour
//{
//    private void Start()
//    {
//        if (NetworkManager.Singleton != null)
//        {
//            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//        }
//    }

//    private void OnClientConnected(ulong clientId)
//    {
//        if (!IsServer) return; // Only the server should spawn players

//        Transform spawnPoint = PlayerSpawner.Instance.GetNextSpawnPoint();
//        if (spawnPoint == null)
//        {
//            Debug.LogError("Spawn point is null! Check PlayerSpawner.");
//            return;
//        }

//        GameObject playerInstance = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, spawnPoint.position, spawnPoint.rotation);

//        // Make sure each player is assigned to their client correctly
//        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

//        Debug.Log($"Spawned player {clientId} at {spawnPoint.position}");
//    }
//}

