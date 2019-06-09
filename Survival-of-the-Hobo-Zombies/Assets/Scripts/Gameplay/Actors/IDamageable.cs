using UnityEngine;
using System.Collections;

public interface IDamageable {

    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);
    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, Vector3 originalPosition);
    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit);
    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit, Vector3 originalPosition);
    void TakeDamage(float damage, bool isCrit);
    void Heal(float amount);
}
