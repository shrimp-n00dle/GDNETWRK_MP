using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OrbSpawner : NetworkBehaviour
{
    public static OrbSpawner Instance;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private GameObject spawnParent;
    private List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        NetworkManager.Singleton.AddNetworkPrefab(orbPrefab);

        for (int i = 0; i < spawnParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnParent.transform.GetChild(i));
        }
    }

    public void SpawnAllOrbs()
    {
        for(int i = 0; i < spawnPoints.Count;i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject orbInstance = Instantiate(orbPrefab, spawnPoint.position, spawnPoint.rotation);
            orbInstance.GetComponent<NetworkObject>().Spawn();
            
        }
        Debug.Log("Spawned all orbs!");


       // Transform spawnPoint = GetRandomSpawnPoint();
       // GameObject fruitInstance = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation);
       // fruitInstance.GetComponent<NetworkObject>().Spawn();
        //Debug.Log("Mystic fruit spawned");

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
