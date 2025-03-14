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
    [SerializeField] private GameObject spawn;
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

        for(int i = 0; i < spawn.transform.childCount; i++)
        {
            spawnPoints.Add(spawn.transform.GetChild(i));
        }
        
    }

    public void SpawnMysticFruit()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        GameObject fruitInstance = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation);
        //layerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        fruitInstance.GetComponent<NetworkObject>().Spawn();
        Debug.Log("Mystic fruit spawned");

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomIndex];
    }
}
