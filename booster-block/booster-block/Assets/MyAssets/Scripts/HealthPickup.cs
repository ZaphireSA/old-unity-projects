using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public float health = 20f;
    [SerializeField]
    GameObject healthPickupEffect;

    Color _color = Color.white;
    Cube player;
    private void Start()
    {
        player = FindObjectOfType<Cube>();
    }

    public void SetColor(Color col)
    {
        _color = col;
        GetComponent<Renderer>().material.color = _color;
        GetComponentInChildren<ParticleSystemRenderer>().material.color = _color;
    }

    private void FixedUpdate()
    {
        if (transform.position.x <= player.transform.position.x - 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Cube>() != null)
        {
            other.GetComponent<Cube>().AddHealth(health);
            FindObjectOfType<GameManager>().HealthPickedUp();
            var healthEffect = Instantiate(healthPickupEffect, transform.position, transform.rotation);
            healthEffect.GetComponent<ParticleSystemRenderer>().material.color = _color;
            var shake = CameraShakePresets.Explosion;
            shake.ScaleMagnitude /= 2;
            shake.ScaleRoughness /= 2;
            //shake.ScaleMagnitude = collision.impulse.magnitude / 5;
            CameraShaker.Instance.Shake(shake);
            Destroy(gameObject);
        }
    }
}
