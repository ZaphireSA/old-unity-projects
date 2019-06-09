using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text txtHealth;
    public Text txtCurrentWeapon;
    public Text txtAlternateWeapon;
    public Text txtAmmo;
    public Text txtExtraAmmo;
    public Text txtCoins;
    public Text txtScrap;
    public Image imgHealthBar;


    Player player;
    GameData gameData;


    void Awake()
    {

    }

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        player.OnDamaged += UpdateHealth;
        player.weaponController.OnWeaponChanged += UpdateWeapon;
        UpdateHealth(player.health);
        gameData = GameData.instance;
        gameData.CoinsChange += UpdateCoins;
        gameData.ScrapChange += UpdateScrap;

        UpdateCoins(gameData.CoinAmount());
        UpdateScrap(gameData.ScrapAmount());
    }

    void UpdateCoins(int amount)
    {
        txtCoins.text = amount.ToString();
        if (txtCoins.GetComponent<Animator>())
            txtCoins.GetComponent<Animator>().SetTrigger("Pop");
    }

    void UpdateScrap(int amount)
    {
        txtScrap.text = amount.ToString();
        if (txtScrap.GetComponent<Animator>())
            txtScrap.GetComponent<Animator>().SetTrigger("Pop");
    }

    void UpdateWeapon(Weapon newWeapon)
    {
        if (newWeapon != null)
        {
            if (txtCurrentWeapon.GetComponent<Animator>() && txtCurrentWeapon.text != newWeapon.weaponName)
                txtCurrentWeapon.GetComponent<Animator>().SetTrigger("Pop");
            txtCurrentWeapon.text = newWeapon.weaponName;
            txtAmmo.text = newWeapon.GetAmmoInfo()[0];
            txtExtraAmmo.text = newWeapon.GetAmmoInfo()[2];
        } else
        {
            txtCurrentWeapon.text = "Hands";
            txtAmmo.text = "--";
            txtExtraAmmo.text = "";
            if (txtCurrentWeapon.GetComponent<Animator>())
                txtCurrentWeapon.GetComponent<Animator>().SetTrigger("Pop");
        }

        Weapon alternateWeapon = player.weaponController.selectedIndex == 0 ? player.weaponController.weapon2 : player.weaponController.weapon1;
        if (alternateWeapon != null)
        {
            if (txtAlternateWeapon.GetComponent<Animator>() && txtAlternateWeapon.text != alternateWeapon.weaponName)
                txtAlternateWeapon.GetComponent<Animator>().SetTrigger("Pop");
            txtAlternateWeapon.text = alternateWeapon.weaponName;
        } else
        {
            txtAlternateWeapon.text = "";
            if (txtAlternateWeapon.GetComponent<Animator>())
                txtAlternateWeapon.GetComponent<Animator>().SetTrigger("Pop");
        }
        
    }

    void UpdateHealth(float amount)
    {
        txtHealth.text = amount.ToString();
        if (txtHealth.GetComponent<Animator>())
            txtHealth.GetComponent<Animator>().SetTrigger("Pop");

        imgHealthBar.fillAmount = amount / player.startingHealth;
    }
	
}
