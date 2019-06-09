using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LivingEntity : MonoBehaviour, IDamageable {

    [SerializeField]
    public float startingHealth;
    public float health;
    [SerializeField]
    public float walkSpeed = 5f;
    [SerializeField]
    public float runSpeed = 5f;
    [SerializeField]
    protected float jumpSpeed = 10f;
    protected bool isGrounded = true;
    protected bool isAlive = true;

    public event System.Action OnDeath;
    public event System.Action<float> OnDamaged;

    public LayerMask collisionMask;

    public Transform getHitParticles;
    public Transform deathEffect;

    public Transform headBone;
    public Transform beardBone;
    public Transform chestBone;
    public Transform pelvisBone;
    public Transform armLeftBone;
    public Transform armRightBone;
    
    public Transform headPiece;
    public Transform beardPiece;
    public Transform chestPiece;
    public AudioClip getHitClip;

    AudioSource audioSource;

    public Popup3D popup3dPrefab;

    public Animator anim;

    protected virtual void Start()
    {
        
        health = startingHealth;
        if (OnDamaged != null)
            OnDamaged(health);

        if (headPiece != null && headBone != null)
        {
            Transform newHeadPiece = Instantiate(headPiece, headBone.transform.position, headBone.transform.rotation) as Transform;
            newHeadPiece.transform.SetParent(transform);
            newHeadPiece.GetComponent<GearAttacher>().AttachToBones(this);
        }

        if (beardPiece != null && beardBone != null)
        {
            Transform newBeardPiece = Instantiate(beardPiece, beardBone.transform.position, beardBone.transform.rotation) as Transform;
            newBeardPiece.transform.SetParent(beardBone);
        }

        if (chestPiece != null && chestBone != null)
        {
            Transform newChestPiece = Instantiate(chestPiece, chestBone.transform.position, chestBone.transform.rotation) as Transform;
            newChestPiece.transform.SetParent(transform);
            newChestPiece.GetComponent<GearAttacher>().AttachToBones(this);
        }
        audioSource = GetComponent<AudioSource>();
        SetKinematic(true);
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage, false);
        GetHitEffects(hitPoint, hitDirection, damage);
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, Vector3 originalPosition)
    {
        TakeDamage(damage, false);
        GetHitEffects(hitPoint, hitDirection, damage);
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit)
    {
        TakeDamage(damage, isCrit);
        GetHitEffects(hitPoint, hitDirection, damage, isCrit);
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit, Vector3 originalPosition)
    {
        TakeDamage(damage, isCrit);
        GetHitEffects(hitPoint, hitDirection, damage, isCrit);
    }

    public virtual void TakeDamage(float damage, bool isCrit)
    {                
        health -= damage;
        if (OnDamaged != null)
            OnDamaged(health);        
        if (health <= 0 && isAlive)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, startingHealth);
        if (OnDamaged != null)
            OnDamaged(health);
    }

    void GetHitEffects(Vector3 hitPoint, Vector3 hitDirection, float damage, bool isCrit = false)
    {
        if (!isAlive) return;
        if (getHitParticles != null)
        {
            Quaternion spawnRot = Quaternion.LookRotation(-hitDirection);
            Instantiate(getHitParticles, hitPoint, spawnRot);
        }

        if (popup3dPrefab != null)
        {
            Popup3D newPopup = Instantiate(popup3dPrefab, hitPoint, Quaternion.identity);
            newPopup.SetInfo(damage.ToString(), isCrit ? Color.red : Color.white);
        }

        if (audioSource != null && getHitClip != null) audioSource.PlayOneShot(getHitClip);
    }

    void SetKinematic(bool newValue)
    {
        List<Collider> colliders = new List<Collider>(GetComponentsInChildren<Collider>());
        List<Collider> parentColliders = new List<Collider>(GetComponents<Collider>());
        foreach (Collider col in colliders)
        {
            if (!parentColliders.Contains(col))
                col.enabled = !newValue;
        }

        List<Rigidbody> bodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        List<Rigidbody> parentBodies = new List<Rigidbody>(GetComponents<Rigidbody>());
        foreach(Rigidbody rb in bodies)
        {
            if (!parentBodies.Contains(rb))
                rb.isKinematic = newValue;
        }

        
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        isAlive = false;
        if (OnDeath != null)
        {
            OnDeath();
        }
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position + Vector3.up, Quaternion.identity);
        }

        SetKinematic(false);
        anim.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<SkinnedMeshRenderer>().updateWhenOffscreen = true;
        GameObject.Destroy(gameObject,5);
    }
}
