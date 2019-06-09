using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    Rigidbody rb;
    [SerializeField]
    float jumpSpeed = 10f;
    [SerializeField]
    float moveSpeed = 2f;
    [SerializeField]
    GameObject poofEffect, damageEffect, changeEffect, destroyedWallPrefab, breakEffect;

    public float maxVelocity = 5;
    public float currentMaxVelocity = 1;
    public float initialMaxVelocity = 10;
    public float maxVelocityPerChange = 2;

    public float health = 200f;
    float initialHealth;
    
    public bool isActive = false;
    public TrailRenderer trail;

    Color color = Color.white;

    public float cubeMaxSize = 2f;
    public float cubeMinSize = 0.3f;
    GameManager gm;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
        rb.isKinematic = true;
        initialHealth = health;
        currentMaxVelocity = initialMaxVelocity;
	}

    public void Activate()
    {
        rb.isKinematic = false;
        isActive = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (health <= 0 || !isActive) return;

        

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            health -= 1;
            gm.HealthUpdate(health);
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            
            var jumpEffect =Instantiate(poofEffect, transform.position + new Vector3(0, 0, 0), Quaternion.identity, transform);            
            var jumpEffectParticleList = jumpEffect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < jumpEffectParticleList.Length; i++)
            {
                jumpEffectParticleList[i].GetComponent<ParticleSystemRenderer>().material.color = color;
            }
            
            
        }
	}

    public void FixedUpdate()
    {
        if (health <= 0 || !isActive) return;
        var velocity = Vector3.Dot(rb.velocity, Vector3.right);

        //var curMax = Mathf.Lerp(initialMaxVelocity, maxVelocity, Mathf.Clamp(transform.position.x, 0, 2000) / 2000);
        

        if (velocity < maxVelocity &&  velocity < currentMaxVelocity)
        {
            rb.AddForce(Vector3.right * (moveSpeed + (transform.position.x / 300)) * Time.deltaTime, ForceMode.Force);            
        }

        float size = Mathf.Lerp(cubeMinSize, cubeMaxSize, health / initialHealth);
        transform.localScale = new Vector3(size, size, size);
        //var size = new Vector3();
    }

    public void AddHealth(float amount)
    {
        //if (health <= 0) return;
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        gm.HealthUpdate(health);
    }

    public void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
        trail.startColor = color;
        trail.endColor = color;
         
        
        var changeEffectItem = Instantiate(changeEffect, transform.position, transform.rotation);
        var particleEffectList = changeEffectItem.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particleEffectList.Length; i++)
        {
            particleEffectList[i].GetComponent<ParticleSystemRenderer>().material.color = this.color;
        }

        var shake = CameraShakePresets.Bump;        
        CameraShaker.Instance.Shake(shake);
        AddHealth(10);

        this.color = color;

        if (transform.position.x > 10)
        {
            currentMaxVelocity = Mathf.Clamp(currentMaxVelocity + maxVelocityPerChange, initialMaxVelocity, maxVelocity);
            rb.AddForce(Vector3.right * rb.mass * ((currentMaxVelocity + 3) - rb.velocity.x), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.collider.tag == "Wall")
        {
            var damageParticles = Instantiate(damageEffect, collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));            
            var particleEffectList = damageParticles.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleEffectList.Length; i++)
            {
                particleEffectList[i].GetComponent<ParticleSystemRenderer>().material.color = color;
            }
            var wall = collision.collider.GetComponent<Wall>();
            var shake = CameraShakePresets.Bump;
            shake.ScaleMagnitude = collision.impulse.magnitude / 5;
            CameraShaker.Instance.Shake(shake);
            health -= wall.isMoving ? collision.impulse.magnitude : collision.impulse.magnitude * 2;
            health = Mathf.Clamp(health, 0, 100);
            gm.HealthUpdate(health);

            var newPos = collision.collider.transform.position;
            var destroyedPos = newPos;
            var displacement = Mathf.Clamp(collision.impulse.magnitude / 10, 0.2f, 3f);
            var effectPos = newPos;
            
            if (newPos.y > transform.position.y)
            {
                destroyedPos -= new Vector3(0, (collision.collider.transform.localScale.y/2) - (displacement/2), 0);
                newPos.y += displacement;
                wall.UpdateOriginalPos(+displacement);
                effectPos = destroyedPos + new Vector3(0, displacement / 2, 0);
            } else
            {
                destroyedPos += new Vector3(0, (collision.collider.transform.localScale.y / 2) - (displacement / 2), 0);
                newPos.y -= displacement;
                wall.UpdateOriginalPos(-displacement);
                effectPos = destroyedPos - new Vector3(0, displacement / 2, 0);
            }            
            collision.collider.transform.position = newPos;
            

            var newBlock = Instantiate(destroyedWallPrefab, destroyedPos, Quaternion.identity);
            var newScale = new Vector3(newBlock.transform.localScale.x, displacement, newBlock.transform.localScale.z);
            newBlock.transform.localScale = newScale;
            newBlock.GetComponent<Renderer>().material.color = collision.collider.GetComponent<Renderer>().material.color;

            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            newBlock.GetComponent<Rigidbody>().AddForce(transform.right * collision.impulse.y * 100);

            var breakEffectObj = Instantiate(breakEffect, effectPos, Quaternion.identity);

            var breakEffectList = breakEffectObj.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < breakEffectList.Length; i++)
            {
                breakEffectList[i].GetComponent<ParticleSystemRenderer>().material.color = collision.collider.GetComponent<Renderer>().material.color;
            }
        }
    }
}
