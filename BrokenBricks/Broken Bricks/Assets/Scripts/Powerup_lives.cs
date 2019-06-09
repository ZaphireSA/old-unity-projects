using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_lives : Powerup
{

    public override void PickedUp()
    {
        //Write code here when picked up
        GameManager.instance.ballsLeft += 1;
        GameManager.instance.UpdateBallsUI();
        base.PickedUp();
    }

}
