using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager Instance;
    private Dictionary<ulong, GameObject> connectedClients = new Dictionary<ulong, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
 
        Debug.Log($"Client {clientId} connected. Total clients: {NetworkManager.Singleton.ConnectedClients.Count}");

        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            PlayerSpawner.Instance.EnableAllPlayerMovementsClientRpc();
        //    FruitSpawner.Instance.SpawnMysticFruit();
            GameManager.Instance.HasTimerStarted = true;
            OrbSpawner.Instance.SpawnAllOrbs();
        }
        else
        {
            PlayerSpawner.Instance.DisableAllPlayerMovementsClientRpc();
        }

    }

}