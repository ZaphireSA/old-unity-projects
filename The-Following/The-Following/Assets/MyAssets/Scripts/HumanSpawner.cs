using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour {

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject humanPrefab;
    [SerializeField] float minSpawnTime = 10f;
    [SerializeField] float maxSpawnTime = 30f;


    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            if (!DetectThreat())
            {
                Instantiate(humanPrefab, spawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    bool DetectThreat()
    {
        var player = FindObjectOfType<Player>();
        if (player == null) return false;
        var distance = Vector3.Distance(transform.position, player.transform.position);
        return distance < 8f;
    }


}
