using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_Spawner : MonoBehaviour
{

    [SerializeField]
    GameObject spawnPrefab;
    [SerializeField]
    int spawnAmount = 5;

    void Start()
    {     
        for (int i = 0; i < 5; i++)
        {
            var maxRandom = Mathf.Min(4, i);
            var randomOffset = new Vector3(Random.Range(0, maxRandom), 0, Random.Range(0, maxRandom));
            Instantiate(spawnPrefab, transform.position + randomOffset, Random.rotation);
        }
        Destroy(gameObject);
    }

}
