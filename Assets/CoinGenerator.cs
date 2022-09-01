using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int spawnVariant;
    [SerializeField] private int coinSpawnChancePercent;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void Spawn()
    {
        int coinToSpawn = Random.Range(3, 10);

        for (int i = 0; i < coinToSpawn; i++)
        {
            spawnVariant++;
            if (Random.Range(1, 100) <= coinSpawnChancePercent)
            {
                Instantiate(coinPrefab, new Vector3(transform.position.x + spawnVariant, transform.position.y, transform.position.z), transform.rotation);
            }
        }
    }

}
