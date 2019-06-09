using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStateManager : LivingEntity
{

    public Transform target;
    public EnemyState currentState;

    public ERoamState roamState;
    public EChaseState chaseState;
    public ESearchState searchState;
    public EnemyState attackState;

    public Transform eyesPoint;
    public enum AttackType { Melee, Ranged };
    public AttackType attackType = AttackType.Melee;

    public float attackDistanceThreshold = 0.5f;
    public float timeBetweenAttacks = 1.0f;
    public float damage = 1f;

    public float myCollisionRadius;
    public float targetColissionRadius;

    public float maxRoamDistance = 6f;
    public float minRoamDistance = 2f;
    public float maxRoamWait = 2f;
    public Vector3 originalPos = Vector3.zero;

    public float fovAngle = 110f;
    public float fovRange = 10f;
    public Vector3 targetLastSeenPos;
    public float hearingRange = 5f;
    public LayerMask visionMask;

    public AudioClip audioRoam;
    public AudioClip audioChase;
    public AudioSource audioSource;

    public bool isRaged = false;

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navAgent;

    public MeshRenderer meshRendererFlag;

    private void Awake()
    {
        roamState = gameObject.AddComponent<ERoamState>();
        chaseState = gameObject.AddComponent<EChaseState>();
        searchState = gameObject.AddComponent<ESearchState>();

        if (GetComponent<EAttackState>() != null)
        {
            attackState = GetComponent<EAttackState>();
        } else if (GetComponent<ERangedAttackState>() != null)
        {
            attackState = GetComponent<ERangedAttackState>();
        } else
        {
            attackState = gameObject.AddComponent<EAttackState>();
        }

        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        audioSource = GetComponent<AudioSource>();
    }    

    protected override void Start()
    {
        base.Start();
        originalPos = transform.position;
        this.OnDeath += DeathEffects;

        StartCoroutine(DelayedStart());
    }

     void DeathEffects()
    {
        if (currentState != null)
            currentState.ExitState();
    }

    void OnDestroy()
    {
        if (currentState != null)
            currentState.ExitState();
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeState(roamState);
    }

    public void ChangeState(EnemyState newState)
    {
        if (isAlive == false) return;
        if (currentState != null)
            currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public override void Die()
    {
        base.Die();
        currentState.enabled = false;
        navAgent.enabled = false;
        audioSource.Stop();
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection, bool isCrit, Vector3 originalPosition)
    {
        base.TakeHit(damage, hitPoint, hitDirection, isCrit, originalPosition);
        currentState.TakeDamage(originalPosition);
    }

    public override void TakeDamage(float damage, bool isCrit)
    {
        base.TakeDamage(damage, isCrit);
        //anim.SetTrigger("GetHit");
    }

    public void HearSound(Vector3 location, float soundRange)
    {
        if (Vector3.Distance(transform.position, location) < soundRange + hearingRange)
        {
            currentState.HeardSomething(location);
        }
    }

    void Update()
    {
        if (currentState != null && isAlive)
        {
            currentState.UpdateState();
        }
    }

    public bool GetClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {            
            target = player.transform;
            return true;
        }
        return false;
    }

    public bool LookForTarget(float addedRange = 0f) 
    {
        if (target == null)
        {

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                Vector3 direction = player.transform.position - eyesPoint.transform.position;
                float angle = Vector3.Angle(direction, eyesPoint.transform.forward);
                if (angle < fovAngle / 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(eyesPoint.transform.position, direction.normalized, out hit, fovRange + addedRange,visionMask) && hit.collider.CompareTag("Player"))
                    {
                        target = player.transform;
                        targetColissionRadius = target.GetComponent<CapsuleCollider>().radius;
                        target.GetComponent<LivingEntity>().OnDeath += OnTargetDeath;
                        return true;
                    }
                }
            }
        } else
        {
            Vector3 direction = target.transform.position - eyesPoint.transform.position;
            float angle = Vector3.Angle(direction, eyesPoint.transform.forward);
            if (angle < fovAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(eyesPoint.transform.position, direction.normalized, out hit, fovRange, visionMask) && hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnTargetDeath()
    {
        currentState.TargetDeath();
        target = null;
    }
}
