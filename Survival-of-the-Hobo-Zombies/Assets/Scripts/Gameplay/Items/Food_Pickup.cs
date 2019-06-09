using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Pickup : Pickupable {

    public float addedHealth = 5f;

    public override void Interact(Player player)
    {
        base.Interact(player);
        player.Heal(addedHealth);
        Interact();
    }
}
