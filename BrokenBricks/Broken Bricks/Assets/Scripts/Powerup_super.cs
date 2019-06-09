using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_super : Powerup {

    public override void PickedUp()
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (Ball ball in balls)
        {
            ball.SetOnFire();
        }

        base.PickedUp();
    }
}
