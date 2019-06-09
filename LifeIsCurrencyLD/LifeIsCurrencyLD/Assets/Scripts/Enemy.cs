using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Player target;
    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject diePrefab;

    [SerializeField]
    float attackDelay,attackDistance,damage = 2f;
    bool hasSpawned = false;

    [SerializeField]
    GameObject heartPrefab, heartDisplay;

    [SerializeField]
    GameObject brainPrefab;

    [SerializeField]
    Transform heartPosition, brainPosition;    

    bool hasHeart, hasBrain = false;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<Player>();

        hasHeart = Random.Range(0, 10) > 3;
        hasBrain = Random.Range(0, 10) > 3;

        if (!hasHeart) heartDisplay.SetActive(false);

        StartCoroutine(Spawn());
        StartCoroutine(AttackEnemy());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3);
        hasSpawned = true;
    }

    public void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        Instantiate(diePrefab, transform.position, transform.rotation);
        if (hasHeart)
        {
            Instantiate(heartPrefab, heartPosition.transform.position, heartPosition.transform.rotation);
        }

        if (hasBrain)
        {
            Instantiate(brainPrefab, brainPosition.transform.position, brainPosition.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned) return;
        if (target != null) agent.SetDestination(target.transform.position);
        anim.SetBool("IsRunning", agent.velocity.magnitude > 0.2f);
    }

    IEnumerator AttackEnemy()
    {
        while(true)
        {
            if (target != null && Vector3.Distance(transform.position, target.transform.position) < attackDistance)
            {
                if (target != null)
                {
                    target.TakeDamage(damage);
                    AudioManager.instance.Play("SkeletonAttack", transform.position);
                    anim.SetTrigger("Attack");
                }
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
