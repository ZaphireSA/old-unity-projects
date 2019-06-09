using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Pickup : Pickupable {

    public int addedAmmo = 5;

    public override void Interact(Player player)
    {
        base.Interact(player);
        if (player.GetComponent<WeaponController>() != null)
        {
            player.GetComponent<WeaponController>().AddAmmo(addedAmmo);
        }
        Interact();
    }
}
