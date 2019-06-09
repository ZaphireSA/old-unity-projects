using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10;
    public float gravity = 1;
    Rigidbody rbody;

    [SerializeField]
    GameObject[] poopPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        var randomSoundI = Random.Range(1, 3);
        
        AudioManager.instance.Play("Fart" + randomSoundI, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        var speed = transform.forward * moveSpeed;
        speed.y += gravity;
        rbody.velocity = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Enemy>() != null)
        {
            collision.collider.gameObject.GetComponent<Enemy>().Die();
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        } else if (collision.collider.gameObject.GetComponent<Terrain>())
        {
            foreach (var poopPrefab in poopPrefabs)
            {
                Instantiate(poopPrefab, collision.contacts[0].point + new Vector3(0, 0.03f, 0), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
