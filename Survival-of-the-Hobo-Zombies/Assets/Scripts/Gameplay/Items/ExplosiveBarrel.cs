using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    public float health = 10f;
    bool isLit = false;
    public float explodeRadius = 5f;
    public float maxDamage = 10f;

    public Transform flameEffect;
    public Transform explodePrefab;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flameEffect.gameObject.SetActive(false);
        StartCoroutine(OnFire());
    }

    public void Heal(float amount)
    {
        
    }

    IEnumerator OnFire()
    {
        while (!isLit) yield return null;

        while (health > 0)
        {
            TakeDamage(1, false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void FireEffects()
    {
        audioSource.Play();
        flameEffect.gameObject.SetActive(true);
    }

    public void TakeDamage(float damage, bool isCrit)
    {
        if (!isLit) FireEffects();
        isLit = true;
        health -= damage;
        if (health <= 0)
        {
            Explode();
            Destroy(gameObject);
            //TODO: Explode
        }
    }

    public void Explode()
    {
        if(explodePrefab != null)
        {
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
        }

        List<IDamageable> iDamageables = new List<IDamageable>();

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius);
        for (int i = 0; i < objectsInRange.Length; i++)
        {                
            IDamageable iDamageable = objectsInRange[i].GetComponentInParent<IDamageable>();
            if (iDamageable != null && !iDamageable.Equals(this) && !iDamageables.Contains(iDamageable))
            {
                iDamageables.Add(iDamageable);
                float distance = Vector3.Distance(transform.position, objectsInRange[i].transform.position);
                float damage = Mathf.RoundToInt(((explodeRadius - distance) / explodeRadius) * maxDamage);
                iDamageable.TakeHit(damage, objectsInRange[i].transform.position, (transform.position - objectsInRange[i].transform.position));
            }
        }
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage, false);
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit)
    {
        TakeDamage(damage, isCrit);
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, Vector3 originalPosition)
    {
        TakeDamage(damage, false);
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit, Vector3 originalPosition)
    {
        TakeDamage(damage, isCrit);
    }
}
