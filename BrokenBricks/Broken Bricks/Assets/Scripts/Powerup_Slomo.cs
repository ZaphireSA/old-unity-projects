using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Slomo : Powerup {

    [SerializeField]
    GameObject sloMoPrefab;

    [SerializeField]
    Vector3 spawnPos;

    public override void PickedUp()
    {
        Instantiate(sloMoPrefab, spawnPos, Quaternion.Euler(-90f, 0, 0));
        base.PickedUp();
    }
}
