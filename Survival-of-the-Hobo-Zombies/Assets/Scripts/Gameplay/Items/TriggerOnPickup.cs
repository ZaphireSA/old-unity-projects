using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnPickup : SpawnTrigger {


    void OnDestroy()
    {
        Activate();
    }


}
