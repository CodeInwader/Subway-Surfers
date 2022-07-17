using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMapGenerator : MonoBehaviour
{
    public List<GameObject> FreeTiles = new List<GameObject>();
    Vector3 spawnPosition = new Vector3(0, 0, 0);
   
    float z = 198;

   public List<GameObject> OfSpawnedTiles = new List<GameObject>();

    private void Awake()
    {
        int randomNumber = Random.Range(0, FreeTiles.Count);
        
        FreeTiles[randomNumber].SetActive(true);
        FreeTiles[randomNumber].transform.position = spawnPosition;

        GameObject g = FreeTiles[randomNumber];
        FreeTiles.Remove(g);
        OfSpawnedTiles.Add(g);
        

        AddTileToRoad();
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "TileSpawning")
        {
            BackToPool();
        }
        
    }

    void AddTileToRoad()
    {
        //Spawning
        spawnPosition = spawnPosition + new Vector3(0, 0, z);
        int randomNumber = Random.Range(0, FreeTiles.Count);
        GameObject tileToSpawn = FreeTiles[randomNumber];
        CoinManager coinManager = tileToSpawn.GetComponentInChildren<CoinManager>();
        coinManager.ReCycleCoins();

        tileToSpawn.SetActive(true);
        tileToSpawn.transform.position = spawnPosition;
        OfSpawnedTiles.Add(tileToSpawn);
        FreeTiles.Remove(tileToSpawn);

        tileToSpawn.GetComponentInChildren<CoinManager>().ReCycleCoins();
    }

    void BackToPool()
    {
        //Back to pool
        GameObject o = OfSpawnedTiles[0];
        OfSpawnedTiles.Remove(OfSpawnedTiles[0]);
        FreeTiles.Add(o);
        o.SetActive(false);
        AddTileToRoad();
    }
}
