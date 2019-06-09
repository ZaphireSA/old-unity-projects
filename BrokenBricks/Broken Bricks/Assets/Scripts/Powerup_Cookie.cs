using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Cookie : Powerup
{    

    public override void PickedUp()
    {
        StageManager.instance.AddCookies(1);
        GameManager.instance.UpdateCookiesUI();
        base.PickedUp();
    }
}
