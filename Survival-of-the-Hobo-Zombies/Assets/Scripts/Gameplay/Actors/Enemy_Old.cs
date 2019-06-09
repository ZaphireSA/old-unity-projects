using UnityEngine;
using System.Collections;

public class Enemy_Old : LivingEntity {

    public enum State { Idle, Roaming, Chasing, Spawning, Attacking }
    State currentState;

    public enum AttackType { Melee, Ranged }
    public AttackType attackType = AttackType.Melee;

    //public static event System.Action OnDeathStatic;

    UnityEngine.AI.NavMeshAgent pathFinder;
    Transform target;

    LivingEntity targetEntity;

    public float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1.0f;
    public float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetColissionRadius;

    public Transform deathPrefab;

    bool hasTarget;
    bool isRunning = false;

    public float maxRoamDistance = 6f;
    public float minRoamDistance = 2f;
    public float maxRoamWait = 2f;
    float nextRoamTime;
    //Vector3 originalPos = Vector3.zero;
    float fovAngle = 110f;
    float fowRange = 10f;

    void Awake()
    {
        pathFinder = GetComponent<UnityEngine.AI.NavMeshAgent>();

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        //originalPos = transform.position;
        targetColissionRadius = 1;
    }

    protected override void Start()
    {
        base.Start();
        pathFinder.speed = (isRunning) ? runSpeed : walkSpeed;
        currentState = State.Idle;
        anim.SetFloat("speedPercent", 0);

        this.OnDeath += DeathEffects;
        StartCoroutine(DetectPlayer());
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeHit(damage, hitPoint, hitDirection, false);
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit)
    {
        base.TakeHit(damage, hitPoint, hitDirection);
        anim.SetTrigger("getHit");

    }

    void DeathEffects()
    {
        if (deathPrefab != null)
        {
            //spawn deathprefab
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
        anim.SetFloat("speedPercent", 0);
    }

    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime && target != null)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (attackType == AttackType.Melee)
                {
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetColissionRadius, 2))
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        StartCoroutine(AttackMelee());
                    }
                }
            }
        }

        if (currentState == State.Idle)
        {
            if (Time.time > nextRoamTime)
            {
                currentState = State.Roaming;
                StartCoroutine(Roam());
            }
        }
    }

    IEnumerator DetectPlayer()
    {
        while (isAlive)
        {
            if (target == null && !hasTarget && (currentState == State.Idle || currentState == State.Roaming))
            {
                Player[] players = GameObject.FindObjectsOfType<Player>();
                foreach (Player player in players)
                {
                    Vector3 direction = player.transform.position - transform.position;
                    float angle = Vector3.Angle(direction, transform.forward);
                    if (angle < fovAngle / 2)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, fowRange))
                        {
                            hasTarget = true;
                            target = player.transform;
                            target.GetComponent<LivingEntity>().OnDeath += TargetDies;
                            currentState = State.Chasing;
                            isRunning = true;
                            StartCoroutine(Chase());
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void TargetDies()
    {
        hasTarget = false;
        target = null;
    }

    IEnumerator AttackMelee()
    {
        currentState = State.Attacking;
        if (hasTarget)
        {
            anim.SetTrigger("attack");
            pathFinder.enabled = false;
            if (target.GetComponent<LivingEntity>() != null)
            {
                target.GetComponent<LivingEntity>().TakeDamage(damage, false);
            }


            while (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                yield return null;
            }
        }
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    IEnumerator Roam()
    {
        pathFinder.enabled = true;
        anim.SetFloat("speedPercent", (isRunning) ? 1 : 0.5f);
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        Vector3 targetPos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        targetPos *= Random.Range(minRoamDistance, maxRoamDistance);
        float distToTarget = (targetPos - transform.position).magnitude;
        pathFinder.SetDestination(targetPos);

        while (currentState == State.Roaming)
        {
            if (distToTarget < 1f)
            {
                nextRoamTime = Time.time + Random.Range(1, maxRoamWait);
                currentState = State.Idle;
                anim.SetFloat("speedPercent", 0);
                pathFinder.enabled = false;
                break;
            }
            yield return new WaitForSeconds(0.1f);
            distToTarget = (targetPos - transform.position).magnitude;
        }

    }

    IEnumerator Chase()
    {
        anim.SetFloat("speedPercent", (isRunning) ? 1 : 0.5f);
        pathFinder.speed = (isRunning) ? runSpeed : walkSpeed;
        float refreshRate = 0.25f;
        while (target != null && hasTarget && currentState == State.Chasing)
        {
            Vector3 distToTarget = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - distToTarget * (myCollisionRadius + targetColissionRadius + attackDistanceThreshold / 2);
            if (isAlive)
            {
                pathFinder.SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }


}
