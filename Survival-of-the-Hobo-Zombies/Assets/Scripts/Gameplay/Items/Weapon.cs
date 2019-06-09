using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour, IWeapon {

    public int weaponId;
    public string weaponName;
    public float critChance;
    public Animator anim;
    public float critModifier;
    public Transform owner;

    public WeaponController weaponController;
    public int weaponType = 0;
    public Transform weaponDrop;    

    public event System.Action OnUpdateInfo;

    public virtual void TriggerOnUpdateInfo()
    {
        if (OnUpdateInfo != null) OnUpdateInfo();
    }

    public virtual void Shoot()
    {

    }

    public virtual void AlternateShoot()
    {

    }

    public virtual void Aim(Vector3 aimPoint)
    {

    }

    public virtual void Reload()
    {

    }

    public virtual void OnTriggerHold()
    {

    }

    public virtual void OnTriggerRelease()
    {

    }

    public virtual string[] GetAmmoInfo()
    {
        string[] ammoStats = { "∞", "∞", "∞" };
        return ammoStats;
    }
	
}
