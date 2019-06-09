using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Pickup : Pickupable
{

    public Weapon weaponPickup;
    public int ammo = 10;

    public override void Interact(Player player)
    {
        base.Interact(player);
        player.weaponController.PickupWeapon(weaponPickup, ammo);
        Interact();
    }
}
