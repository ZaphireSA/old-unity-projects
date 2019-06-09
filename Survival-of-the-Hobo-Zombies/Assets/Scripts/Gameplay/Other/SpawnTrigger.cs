using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger: MonoBehaviour {

    public SpawnManager[] spawnManagers;

    public void Activate()
    {
        foreach(SpawnManager spawnManager in spawnManagers)
        {
            spawnManager.TriggerSpawns();
        }
    }

}
