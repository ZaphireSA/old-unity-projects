using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : Weapon {
    public enum FireMode { Single, Continues };
    public FireMode fireMode = FireMode.Single;

    public float msBetweenShots = 500f;
    public float minDamagePerShot = 1f;
    public float maxDamagePerShot = 2f;

    public float hitAngle = 80f;
    public float hitRange = 3f;

    public float damage = 2f;

    public float hitDelay = 0.3f;

    float nextHitTime;

    List<GameObject> nearEnemies = new List<GameObject>();

    public AudioClip audioSwing;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Shoot()
    {
        base.Shoot();

        if (Time.time > nextHitTime)
        {
            nextHitTime = Time.time + msBetweenShots / 1000f;
            StartCoroutine(Swing());
        }
    }

    //IEnumerator Swing()
    //{
    //    anim.SetTrigger("Attack");
    //    yield return new WaitForSeconds(hitDelay);

    //    LivingEntity[] livingEntities = GameObject.FindObjectsOfType<LivingEntity>();

    //    foreach (LivingEntity livingEntity in livingEntities)
    //    {
    //        Vector3 direction = (livingEntity.transform.position) - (owner.transform.position);
    //        float angle = Vector3.Angle(direction, owner.transform.forward);

    //        //float angle = Vector3.Angle(owner.transform.forward, direction);
    //        Debug.Log(angle + "," + hitAngle/2);
    //        if (angle < hitAngle / 2)
    //        {
    //            RaycastHit hit;
    //            float distance = Vector3.Distance(transform.position, livingEntity.transform.position);
                
    //            if (distance < 0.5f) {
    //                Debug.Log("CLOSE HIT");
    //                livingEntity.TakeHit(damage, livingEntity.transform.position, -direction);

    //            } else if (Physics.Raycast(owner.transform.position + owner.transform.up, direction.normalized, out hit, hitRange))
    //            {
    //                LivingEntity hitEntity = hit.collider.GetComponentInParent<LivingEntity>();
    //                if (hitEntity != null && livingEntity.Equals(hitEntity))
    //                {
    //                    Debug.Log("FAR HIT");
    //                    livingEntity.TakeHit(damage, hit.point, hit.normal);
    //                }
    //            }
    //        }
    //    }
    //}    

    IEnumerator Swing()
    {
        anim.SetTrigger("Attack");
        audioSource.PlayOneShot(audioSwing);
        yield return new WaitForSeconds(hitDelay);

        IDamageable[] livingEntities = GameObject.FindObjectsOfType<LivingEntity>();
        
        foreach(LivingEntity livingEntity in livingEntities)
        {
            if (owner.GetComponentInParent<LivingEntity>().Equals(livingEntity)) continue;
            float distance = Vector3.Distance(owner.transform.position, livingEntity.transform.position);
            if (distance < hitRange)
            {
                Vector3 direction = new Vector3(livingEntity.transform.position.x, owner.transform.position.y, livingEntity.transform.position.z) - owner.transform.position;
                float angle = Vector3.Angle(owner.transform.forward, direction);
                Debug.Log(angle);
                if (angle < hitAngle / 2)
                {
                    livingEntity.TakeHit(damage, new Vector3(livingEntity.transform.position.x, transform.position.y, livingEntity.transform.position.z), -direction);
                }
            }
        }        

    }

    void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.tag == "Enemy") nearEnemies.Add(col.gameObject);
    }
    void OnTriggerExit(Collider col)
    {
        //if (col.gameObject.tag == "Enemy") nearEnemies.Remove(col.gameObject);
    }

    public override void OnTriggerHold()
    {
        base.OnTriggerHold();
        Shoot();        
    }
}
