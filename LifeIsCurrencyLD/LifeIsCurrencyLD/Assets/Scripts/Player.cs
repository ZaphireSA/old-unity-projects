using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float health = 100f;

    [SerializeField]
    float rotateSpeed = 3.0F;
    [SerializeField]
    float shootDelay = 1;

    CharacterController controller;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    GameObject shootProjectile, shootEffect, deathEffect, poopPrefab, listenerPrefab;

    GameManager gameManager;
    [SerializeField]
    Animator anim;

    Vector3 inputDir = Vector3.zero;
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        gameManager.HealthUpdated(health);
        if (health <= 0)
        {
            Instantiate(listenerPrefab, transform.position, transform.rotation);
            AudioManager.instance.Play("Death", transform.position);
            Instantiate(deathEffect, transform.position, transform.rotation);
            Instantiate(poopPrefab, transform.position, transform.rotation);
            gameManager.PlayerDied();
            Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        gameManager.HealthUpdated(health);
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (transform.position.y < -2.448853f)
        {
            TakeDamage(100);
        }

        var horizontal = Input.GetAxisRaw("Horizontal"); 
        var vertical = Input.GetAxisRaw("Vertical");

        //var horizontal = 1;
        //var vertical = 0;
        inputDir = new Vector3(horizontal * moveSpeed, 0, vertical * moveSpeed);        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //moveDirection = moveDirection.normalized;
        //Debug.Log(inputDir);
        if (inputDir != Vector3.zero)
        {
            
            var slerpRotation = Quaternion.Slerp(Quaternion.LookRotation(inputDir), transform.rotation, rotateSpeed);
            transform.rotation = slerpRotation;
            
        }



        anim.SetBool("IsRunning", inputDir != Vector3.zero);

        controller.SimpleMove(inputDir);        
        
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        anim.SetTrigger("Roll");
        var rand = Random.RandomRange(1, 3);
        if (rand == 1) AudioManager.instance.Play("Uhh1", transform.position);
        else AudioManager.instance.Play("Uhh2", transform.position);
        yield return new WaitForSeconds(shootDelay);

        Instantiate(shootProjectile, shootPoint.transform.position, shootPoint.transform.rotation);
        Instantiate(shootEffect, shootPoint.transform.position, shootPoint.transform.rotation);
        TakeDamage(1);
    }
}
