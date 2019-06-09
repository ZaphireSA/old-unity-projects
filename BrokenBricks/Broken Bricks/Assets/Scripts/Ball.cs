using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ball : MonoBehaviour
{

    public float maxSpeed = 10f;
    public float minSpeed = 5f;
    public float gravity = -660f;

    public float speed = 4f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    Rigidbody rBody;
    public Animator anim;
    public bool isStuck = true;
    GameObject stuckPoint;

    float timeOnZeroZ = 0;
    float timeOnZeroX = 0;
    public float maxFlametime = 6;
    bool isOnFire = false;
    [SerializeField]
    ParticleSystem[] fireParticles;

    [SerializeField]
    Color ColorStart = Color.green;
    [SerializeField]
    Color ColorFire = Color.red;

    [SerializeField]
    Color ColorGlowStart = Color.green;

    [SerializeField]
    Renderer ColorMat;

    [SerializeField]
    Renderer GlowMat;


    bool isNewTouchSinceStart = false;
    bool isTouchDragging = false;

    [SerializeField]
    GameObject hitPrefab;

    public void SetOnFire()
    {
        isOnFire = true;
        StartCoroutine(OnFire());
    }

    IEnumerator OnFire()
    {
        ColorMat.material.color = ColorFire;
        GlowMat.material.SetColor("_TintColor", new Color(ColorFire.r, ColorFire.g, ColorFire.b, 0.3f));
        foreach (var effect in fireParticles)
        {
            effect.Play();
        }
        GetComponent<TrailRenderer>().startColor = ColorFire;
        yield return new WaitForSeconds(maxFlametime);
        ColorMat.material.color = ColorStart;
        GlowMat.material.color = ColorStart;
        GlowMat.material.SetColor("_TintColor", ColorGlowStart);
        GetComponent<TrailRenderer>().startColor = ColorStart;
        isOnFire = false;
        foreach (var effect in fireParticles)
        {
            effect.Stop();
        }
    }

    void Die()
    {
        GameManager.instance.LostBall(gameObject);
        GameObject.Destroy(gameObject);
    }

    void IsStuckOnZ()
    {
        if (timeOnZeroZ > 4)
        {
            Vector3 vel = rBody.velocity + new Vector3(0, 0, -2);

            rBody.velocity = vel;
        }
    }

    void IsStuckOnX()
    {
        if (timeOnZeroX > 4)
        {
            var randomX = Random.Range(0, 2) * 2 - 1;


            Vector3 vel = rBody.velocity + new Vector3(randomX * 2, 0, 0);

            rBody.velocity = vel;
        }
    }

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        GetComponent<TrailRenderer>().startColor = ColorStart;
        ColorMat.material.color = ColorStart;
        GlowMat.material.SetColor("_TintColor", ColorGlowStart);
        foreach (var effect in fireParticles)
        {
            effect.Stop();
        }
    }
    void Start()
    {
        float randomX = Random.Range(-speed, speed);

        var breaker = FindObjectOfType<Breaker>();
        stuckPoint = breaker.ballHolder.gameObject;

        StartCoroutine(CheckStuckOnZ());
        StartCoroutine(CheckStuckOnX());
        //rBody.velocity = velocity;
    }

    IEnumerator CheckStuckOnZ()
    {
        while (true)
        {
            if (rBody.velocity.z > -1.5 && rBody.velocity.z < 1.5 && !isStuck)
            {
                timeOnZeroZ++;
                IsStuckOnZ();
            }
            else
            {
                timeOnZeroZ = 0;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator CheckStuckOnX()
    {
        while (true)
        {
            if (rBody.velocity.x > -1.5 && rBody.velocity.x < 1.5 && !isStuck)
            {
                timeOnZeroX++;
                IsStuckOnX();
            }
            else
            {
                timeOnZeroX = 0;
            }
            yield return new WaitForSeconds(1);
        }
    }

    void Update()
    {
        if (isStuck)
        {
            if (stuckPoint != null)
            {
                transform.position = stuckPoint.transform.position;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            //isTouchDragging = false;
            //foreach (var touch in Input.touches)
            //{
            //    if (touch.phase == TouchPhase.Began && !isNewTouchSinceStart)
            //    {
            //        isNewTouchSinceStart = true;
            //    }
            //    if (touch.phase == TouchPhase.Moved)
            //    {
            //        isTouchDragging = true;
            //    }

            //    if (!isTouchDragging && touch.phase == TouchPhase.Ended && isNewTouchSinceStart)
            //    {
            //        UnStuck();
            //    }
            //    if (touch.phase == TouchPhase.Ended)
            //    {
            //        isTouchDragging = false;
            //    }
            //}

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                switch(touch.phase)
                {
                    case TouchPhase.Moved:
                        isTouchDragging = true;
                        break;
                    case TouchPhase.Ended:
                        if (!isTouchDragging)
                        {
                            UnStuck();
                        }
                        isTouchDragging = false;
                        break;
                }
            } else
            {
                isTouchDragging = false;
            }

#else
            if (Input.GetMouseButtonDown(0))
            ////if (Input.GetButtonDown("Jump"))
            {
                UnStuck();
            }
#endif
        }
        else
        {
            //rBody.velocity = velocity;

        }
    }

    public void UnStuck()
    {
        var pos = FindObjectOfType<Breaker>().GetTargetPosition();

        velocity = pos - transform.position;
        //velocity = new Vector3(pos.x, 0, speed);


        rBody.velocity = velocity;
        FindObjectOfType<Breaker>().ChangeState(Breaker.BreakerState.Move);
        isStuck = false;
        //GameManager.instance.ActivateSpell();
        //GameManager.instance.HideSpells();
    }

    private void FixedUpdate()
    {
        if (isStuck) return;
        var speedModifier = 1f;
        var slowMo = GameObject.FindWithTag("SloMo");
        if (slowMo && transform.position.z < slowMo.transform.position.z && transform.position.z > slowMo.transform.position.z - 5f) speedModifier = 0.3f;
        speed = Mathf.Clamp(speed - 0.1f, minSpeed, maxSpeed) * speedModifier;
        var tmpGravity = gravity * speedModifier;
        var velo = rBody.velocity + new Vector3(0, 0, -tmpGravity * Time.deltaTime);
        rBody.velocity = velo.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ball")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

        if (collision.collider.tag == "Breaker")
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(2, 1f, 0.1f, 0.2f);
            speed = maxSpeed;
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider == GetComponent<Collider>())
                {
                    //this is th paddles contact point

                    float english = contact.point.x - collision.collider.transform.position.x; // transform.position.x;
                    velocity = new Vector3(english * 1, 0, -velocity.z);

                    rBody.velocity = velocity;
                    break;
                    //contact.otherCollider.rigidbody.AddForce(300f * english, 0, 0);
                }//if
            }

            AudioManager.instance.Play("breaker_hit");
        }

        if (collision.collider.tag == "Wall" || collision.collider.tag == "Breaker" || collision.collider.tag == "Brick")
        {
            if (hitPrefab != null)
            {
                var contact = collision.contacts[0]; // get the first contact point info // find the necessary rotation... 
                var rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);

                Instantiate(hitPrefab, transform.position, rot);
            }
            //velocity.x = -velocity.x;
        }
        else if (collision.collider.tag == "DeathWall")
        {
            Die();
        }

        if (collision.collider.GetComponent<Brick>() != null)
        {
            speed = maxSpeed;
            if (isOnFire)
            {
                rBody.velocity = new Vector3(-collision.relativeVelocity.x, 0, -collision.relativeVelocity.z);
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                collision.collider.GetComponent<Brick>().Damage(100);
            }
            else
            {
                collision.collider.GetComponent<Brick>().Damage();

            }

            anim.SetTrigger("Flash");
        }
        anim.SetTrigger("Flash");

    }
}
