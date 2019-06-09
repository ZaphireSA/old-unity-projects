using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public float speed;
    public Vector3 velocity = new Vector3(0, 0, 0);
    Rigidbody rBody;
    public GameObject pickupEffect;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        velocity = new Vector3(0, 0, -speed);
    }

    public virtual void PickedUp()
    {
        if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);        
        AudioManager.instance.Play("powerup_pickup");
        GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Breaker")
        {
            PickedUp();
        } else if (collider.tag == "DeathWall")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        var speedModifier = 1f;
        var slowMo = GameObject.FindWithTag("SloMo");
        if (slowMo && transform.position.z < slowMo.transform.position.z && transform.position.z > slowMo.transform.position.z - 5f) speedModifier = 0.3f;
        var realVelocity = velocity * speedModifier;        
        rBody.velocity = realVelocity;
    }

}

[System.Serializable]
public class PowerupInfo
{
    public GameObject prefab;
    public int rarityWeight = 1;
}