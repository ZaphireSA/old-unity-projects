using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : Pickupable
{

    public int addedCoins = 0;
    public int addedScrap = 0;

    public override void Interact(Player player)
    {
        base.Interact(player);
        if (addedCoins > 0) GameData.instance.AddCoins(addedCoins);
        if (addedScrap > 0) GameData.instance.AddScrap(addedScrap);
        Interact();
    }
}
