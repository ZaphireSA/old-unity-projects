using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Transform weaponHold;

    Weapon equippedWeapon;
    public Weapon weapon1;
    public Weapon weapon2;

    public Animator anim;    

    public int selectedIndex = 0;

    public Player player;

    public event System.Action<Weapon> OnWeaponChanged;

    void Start()
    {
        EquipWeapon(weapon1, false);
        player = GetComponent<Player>();
    }    

    public void AddAmmo(int amount)
    {
        Gun equippedGun = equippedWeapon as Gun;
        if (equippedGun != null)
        {
            equippedGun.extraAmmo += amount;
        }            
    }

    public void PickupWeapon(Weapon targetWeapon, int extraAmmo = 0)
    {
        Gun targetGun = targetWeapon as Gun;
        if (targetGun != null)
        {
            targetGun.extraAmmo = extraAmmo;
        }
        
        if (equippedWeapon != null)
        {
            DropWeapon();
        }
        if (selectedIndex == 0)
        {
            weapon1 = targetWeapon;                        
        } else if (selectedIndex == 1) {
            weapon2 = targetWeapon;
        }
        EquipWeapon((selectedIndex == 0) ? weapon1 : weapon2, false);
    }

    public void DropWeapon()
    {
        if (equippedWeapon != null)
        {
            if (equippedWeapon.weaponDrop != null)
            {
                Transform weaponDrop = Instantiate(equippedWeapon.weaponDrop, equippedWeapon.transform.position, equippedWeapon.transform.rotation);
                Gun equippedGun = equippedWeapon as Gun;
                if (equippedGun != null)
                {
                    Weapon_Pickup wD = weaponDrop.GetComponent<Weapon_Pickup>();
                    if (wD != null) wD.ammo = equippedGun.extraAmmo + equippedGun.projectilesRemainingInMag;
                }
            }
            Destroy(equippedWeapon.gameObject);
            if (selectedIndex == 0)
            {
                weapon1 = null;
                equippedWeapon = null;
            } else if (selectedIndex == 1)
            {
                weapon2 = null;
                equippedWeapon = null;
            }
            EquipWeapon();            
        }
    }

    public void SwitchBetweenWeapons()
    {

        //This part will ensure that the extra ammo left is kept.        
        Gun equippedGun = equippedWeapon as Gun;
        if (equippedGun != null)
        {
            if ((weapon1 as Gun) != null && selectedIndex == 0)
            {
                ((Gun)weapon1).extraAmmo = equippedGun.extraAmmo + equippedGun.projectilesRemainingInMag;
            }
            else if ((weapon2 as Gun) != null && selectedIndex == 1)
            {
                ((Gun)weapon2).extraAmmo = equippedGun.extraAmmo + equippedGun.projectilesRemainingInMag;
            }
        }
        selectedIndex = (selectedIndex == 0) ? 1 : 0;
        EquipWeapon();
    }

    public void EquipWeapon()
    {       
        EquipWeapon((selectedIndex == 0) ? weapon1 : weapon2, false);
    }

    void TriggerWeaponChanged()
    {
        if (OnWeaponChanged != null)
            OnWeaponChanged(equippedWeapon);
    }

    public void EquipWeapon(Weapon weaponToEquip, bool resetAmmo)
    {
        if (weaponToEquip != null)
        {
            if (equippedWeapon != null)
            {
                Destroy(equippedWeapon.gameObject);
            }
            equippedWeapon = Instantiate(weaponToEquip, weaponHold.position, weaponHold.rotation) as Weapon;
            equippedWeapon.transform.parent = weaponHold;
            equippedWeapon.anim = anim;
            equippedWeapon.owner = this.transform;
            equippedWeapon.weaponController = this;
            anim.SetInteger("WeaponType", equippedWeapon.weaponType);
            equippedWeapon.OnUpdateInfo += TriggerWeaponChanged;
            
        }
        else
        {
            if (equippedWeapon != null)
            {
                Destroy(equippedWeapon.gameObject);
                equippedWeapon = null;
            }
            anim.SetInteger("WeaponType", 0);
        }
        TriggerWeaponChanged();
    }

    public void OnTriggerHold()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.OnTriggerRelease();
        }
    }

    public float WeaponHeight
    {
        get
        {
            return weaponHold.position.y;
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.Aim(aimPoint);
        }
    }

    public void Reload()
    {
        if (equippedWeapon != null) {
            equippedWeapon.Reload();
        }
    }

}
