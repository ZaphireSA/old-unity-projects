using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int health = 2;
    public int startingHealth = 2;
    public int scorePointsOnHit = 5;
    public int scorePointsOnDeath = 10;
    public Animator anim;
    public int dropChance = 0;

    public GameObject deathPrefab;


    void Start()
    {
        GameManager.instance.BrickAdded();
        health = startingHealth;

        var type = Random.Range(1, 5);
        anim.SetInteger("Type", type);
    }


    void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    public void Damage(int damage = 1)
    {
        var points = Mathf.Clamp(damage, damage, health) * scorePointsOnHit;

        health -= damage;
        //anim.SetTrigger("Flash");
        anim.SetFloat("Health", ((float)health / (float)startingHealth));
        

        GameManager.instance.AddScore(points);

        if (health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            if (deathPrefab != null) {
                Destroy(Instantiate(deathPrefab, transform.position, Quaternion.identity), 3);
            }
            EZCameraShake.CameraShaker.Instance.ShakeOnce(7, 4f, 0.1f, 1f);
            AudioManager.instance.Play("brick_hit");
            //GameManager.instance.AddScore(scorePointsOnDeath);
            DropPowerUp();
            GameManager.instance.BrickDestroyed();

            for (int i = 0; i < scorePointsOnDeath; i++)
            {
                var maxRandom = Mathf.Min(4, i);
                var randomOffset = new Vector3(Random.Range(0, maxRandom), 0, Random.Range(0, maxRandom));
                Instantiate(GameManager.instance.collectEffect, transform.position + randomOffset, Random.rotation);
            }

            Destroy(gameObject);
            
        }
        else if (health >= 1)
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2, 1f, 0.1f, 0.5f);
            AudioManager.instance.Play("ball_hit");
        }
    }
    public void DropPowerUp()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber < dropChance)
        {
            var powerup = GameManager.instance.GetRandomPowerUp();
            if (powerup != null)
            {
                Instantiate(powerup.prefab, transform.position, Quaternion.identity);
            }
        }
    }


}
