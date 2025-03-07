using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] public GameObject playerPrefab;
    private int spawnIndex = 0;

    private void Awake()
    {
        Instance = this;
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

    //public GameObject GetPlayerPrefab()
    //{
    //    return playerPrefab;
    //}
}

//using UnityEngine;
//using System.Collections.Generic;

//public class PlayerSpawner : MonoBehaviour
//{
//    public static PlayerSpawner Instance;
//    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
//    private int spawnIndex = 0;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    public Transform GetNextSpawnPoint()
//    {
//        if (spawnPoints.Count == 0)
//        {
//            Debug.LogError("No spawn points assigned!");
//            return null;
//        }

//        Transform spawnPoint = spawnPoints[spawnIndex];

//        // Move to the next spawn point for the next player
//        spawnIndex = (spawnIndex + 1) % spawnPoints.Count;

//        Debug.Log($"Assigned spawn point {spawnIndex} at {spawnPoint.position}");
//        return spawnPoint;
//    }
//}
