using UnityEngine;
using System.Collections;

public interface IWeapon {

    void Shoot();
    void AlternateShoot();
    void Aim(Vector3 aimPoint);
    void Reload();
    void OnTriggerHold();
    void OnTriggerRelease();
    string[] GetAmmoInfo();
}
