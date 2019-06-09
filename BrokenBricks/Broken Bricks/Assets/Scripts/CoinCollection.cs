using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : Powerup {

    public override void PickedUp()
    {
        StageManager.instance.AddCookies(1);
        GameManager.instance.UpdateCookiesUI();
        base.PickedUp();
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(100 * Time.deltaTime, 0, 100 * Time.deltaTime));
    }

}
