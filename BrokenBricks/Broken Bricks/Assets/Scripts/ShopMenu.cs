using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour {

    [SerializeField]
    GameObject ButtonPrefab;
    [SerializeField]
    Transform ButtonHolder;
    [SerializeField]
    public TextMeshProUGUI TxtCoins;

    public void CreateMenu()
    {
        TxtCoins.text = StageManager.instance.playerData.Cookies.ToString();

        //foreach (Transform child in ButtonHolder.transform)
        //{
        //    Destroy(child.gameObject);
        //}


        //foreach (var spell in StageManager.instance.spells)
        //{
        //    var newBtn = Instantiate(ButtonPrefab, ButtonHolder);
        //    newBtn.GetComponent<ShopButton>().spell = spell;
        //}
    }    
}
