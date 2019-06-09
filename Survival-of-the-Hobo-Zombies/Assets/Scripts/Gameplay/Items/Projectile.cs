using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public Transform deathPrefab;
    float speed = 10f;
    float damage = 1f;

    public Transform solidHitAudio;
    public Transform fleshHitAudio;

    bool isCrit = false;

    float lifetime = 3;
    float skinWidth = 0.1f;

    Vector3 originalPosition;

    void Start()
    {        
        Destroy(gameObject, lifetime);
        originalPosition = transform.position;
    }

    public void SetDamage(float _damage, bool _isCrit)
    {
        damage = _damage;
        isCrit = _isCrit;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit);
        }
    }

    void OnHitObject(Collider c, RaycastHit hit)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            bool headshot = false;
            float curDamage = damage;
            if (c.GetType().Equals(typeof(SphereCollider)))
            {
                headshot = true;
                curDamage *= 2;
            }
            damageableObject.TakeHit(curDamage, hit.point, transform.forward, headshot, originalPosition);
        } else if (c.GetComponentInParent<Rigidbody>())
        {
            c.GetComponentInParent<Rigidbody>().AddForceAtPosition(transform.forward * 100, transform.position);
        }

        if (deathPrefab != null && c.GetComponent<LivingEntity>() == null)
        {
            Quaternion spawnRot = Quaternion.LookRotation(hit.normal);
            Instantiate(deathPrefab, hit.point, spawnRot);
        }

        if (c.GetComponent<LivingEntity>() != null)
        {
            if (fleshHitAudio != null) Instantiate(fleshHitAudio, transform.position, transform.rotation);
        } else
        {
            if (solidHitAudio != null) Instantiate(solidHitAudio, transform.position, transform.rotation);
        }
                
        GameObject.Destroy(gameObject);
    }
}
