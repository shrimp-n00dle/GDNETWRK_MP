using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

public class FruitSpawner : NetworkBehaviour
{
    public static FruitSpawner Instance;
    [SerializeField] public GameObject fruitPrefab;
    [SerializeField] private GameObject spawnParent;
    private List<Transform> spawnPoints = new List<Transform>();


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
        NetworkManager.Singleton.AddNetworkPrefab(fruitPrefab);

        for(int i = 0; i < spawnParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnParent.transform.GetChild(i));
        }
        
    }

    private void SpawnMysticFruit()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject fruitInstance = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation);
        //layerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        fruitInstance.GetComponent<NetworkObject>().Spawn();
        // Debug.Log("Mystic fruit spawned");
        ShowFruitSpawnedUIClientRpc();
    }

    private Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform transform = spawnPoints[randomIndex];
        spawnPoints.RemoveAt(randomIndex);
        return transform;
    }



    public void CheckAllPlayerPoints()
    {
        if(!IsServer) return;

        int P1points = 0;
        int P2points = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            ulong clientId = client.ClientId;
            GameObject playerObject = client.PlayerObject?.gameObject;

            if (playerObject != null)
            {
                Player playerScript = playerObject.GetComponent<Player>();
                int playerPoints = playerScript.points.Value;

                if (client.ClientId == 0)
                {
                    P1points = playerPoints;
                }
                else
                {
                    P2points = playerPoints;
                }

              //  Debug.Log($"Player {clientId} has {playerPoints} points.");
            }
        }

        // Debug.Log("Total points: " +  (P1points + P2points));

        if ((P1points + P2points) % 15 == 0)
        {
            this.SpawnMysticFruit();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckIfFruitCanSpawnServerRpc()
    {
        int P1Points = GameManager.Instance.CheckPlayerPoints(1);
        int P2Points = GameManager.Instance.CheckPlayerPoints(2);

        if ((P1Points + P2Points) % 15 == 0)
        {
            this.SpawnMysticFruit();
        }
    }

    [ClientRpc(RequireOwnership = false)]
    public void ShowFruitSpawnedUIClientRpc()
    {
        UIManager.Instance.ShowFruitSpawnPanel = true;
    }

  
}
