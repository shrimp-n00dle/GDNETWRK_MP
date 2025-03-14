using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] public GameObject playerPrefab;
    private int spawnIndex = 0;

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
        NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);
    }

    public Transform GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return null;
        }

        Transform spawnPoint = spawnPoints[spawnIndex];
        spawnIndex = (spawnIndex + 1) % spawnPoints.Count; // Cycle through spawn points
        return spawnPoint;
    }

    [ClientRpc]
    public void ModifyPlayerMovementsClientRpc()
    {
        if (!IsServer) return;

        if(NetworkManager.Singleton.ConnectedClients.Count == 2) {
            GameObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            localPlayer.GetComponent<PlayerMovement>().enabled = true;

            //for testing only:
            FruitSpawner.Instance.SpawnMysticFruit();
        }
        else
        {
            GameObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            localPlayer.GetComponent<PlayerMovement>().enabled = false;
        }
        
    }

    [ClientRpc]
    public void EnableAllPlayerMovementsClientRpc()
    {
        if (!IsServer) return;

            GameObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            localPlayer.GetComponent<PlayerMovement>().enabled = true;

            //for testing only:
            FruitSpawner.Instance.SpawnMysticFruit();
    }

    [ClientRpc]
    public void DisableAllPlayerMovementsClientRpc()
    {
        if(!IsServer) return;

        GameObject localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        localPlayer.GetComponent<PlayerMovement>().enabled = false;
    }

}