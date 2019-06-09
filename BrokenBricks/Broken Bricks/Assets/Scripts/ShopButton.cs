using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour {

    //[HideInInspector]
    //public Spell spell;

    [SerializeField]
    TextMeshProUGUI Cost, Title, Description, Unlock, BuyDescription;
    [SerializeField]
    Image Icon;
    [SerializeField]
    GameObject PanelUnlock, SlotPrefab, SlotHolder;
    [SerializeField]
    Sprite filledHolderSprite;
    [SerializeField]
    Button BuyBtn;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        //Cost.text = spell.Cost.ToString();
        //Title.text = spell.Name.ToString();
        //Description.text = spell.Description.ToString();
        //Icon.sprite = spell.Icon;

        //var requiredStageName = StageManager.instance.stages[spell.RequiredStage - 1].SceneName;
        //var stageScore = StageManager.instance.GetStageScore(requiredStageName);
        //if (stageScore == 0)
        //{
        //    Unlock.text = "Unlocked at stage " + spell.RequiredStage;
        //}
        //else
        //{
        //    PanelUnlock.SetActive(false);
        //}

        //foreach(Transform child in SlotHolder.transform) {
        //    Destroy(child.gameObject);
        //}

        //var spellCharges = StageManager.instance.GetSpellCharges(spell);

        //if (StageManager.instance.playerData.Cookies < spell.Cost)
        //{
        //    BuyBtn.interactable = false;
        //    BuyDescription.text = "Not enough coins";
        //} else if (spell.MaxCharges == spellCharges) {
        //    BuyBtn.interactable = false;
        //    BuyDescription.text = "Max charges reached";
        //} else
        //{
        //    BuyBtn.interactable = true;
        //    BuyDescription.text = "";
        //}


        
        //for (int i = 0; i < spell.MaxCharges; i++)
        //{
        //    var newSlot = Instantiate(SlotPrefab, SlotHolder.transform);
        //    if (i < spellCharges) newSlot.GetComponent<Image>().sprite = filledHolderSprite;
        //}
    }

    //public void BuyCharge()
    //{
    //    StageManager.instance.BuySpell(spell);
    //    UpdateUI();
    //    FindObjectOfType<ShopMenu>().TxtCoins.text = StageManager.instance.playerData.Cookies.ToString();

    //}

}
