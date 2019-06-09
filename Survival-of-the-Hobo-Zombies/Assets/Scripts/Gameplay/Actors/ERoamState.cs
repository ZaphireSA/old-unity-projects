using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ERoamState : EnemyState
{
    protected override void Start()
    {
        base.Start();
    }

    public override void EnterState()
    {
        isStateActive = true;
        enemy.meshRendererFlag.material.color = Color.blue;
        enemy.navAgent.enabled = true;
        enemy.navAgent.speed = enemy.walkSpeed;
        enemy.anim.SetFloat("SpeedPercent", 0.5f);

        if (enemy.audioSource.clip != enemy.audioRoam)
        {
            enemy.audioSource.clip = enemy.audioRoam;
            enemy.audioSource.Play();
        }

        StartCoroutine(ManualUpdate());
    }

    public override void ExitState()
    {
        isStateActive = false;
        StopAllCoroutines();
    }    

    public override IEnumerator ManualUpdate()
    {
        while (isStateActive)
        {
            //float angle = Random.Range(0.0f, Mathf.PI * 2);
            //Vector3 targetPos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            //targetPos *= Random.Range(enemy.minRoamDistance, enemy.maxRoamDistance);
            //float distToTarget = (targetPos - enemy.transform.position).magnitude;


            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(enemy.minRoamDistance, enemy.maxRoamDistance);
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, enemy.maxRoamDistance, 1);
            Vector3 targetPos = hit.position;
            
              
            if ((enemy.originalPos - enemy.transform.position).magnitude > enemy.maxRoamDistance * 1.5)
            {
                targetPos = enemy.originalPos;
            }

            enemy.navAgent.SetDestination(targetPos);
            enemy.navAgent.speed = enemy.walkSpeed;
            enemy.anim.SetFloat("SpeedPercent", 0.5f);
            enemy.navAgent.Resume();
            
            while (enemy.navAgent.remainingDistance > 0.3f) //Arrived.
            {                
                yield return null;
            }
            //enemy.navAgent.speed = 0;
            enemy.anim.SetFloat("SpeedPercent", 0);
            enemy.navAgent.Stop();
            yield return new WaitForSeconds(Random.Range(1, enemy.maxRoamWait));            
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }    

    public override void UpdateState()
    {
        if (enemy.LookForTarget())
        {
            enemy.ChangeState(enemy.chaseState);
        }

        if (enemy.isRaged)
        {
            if (enemy.GetClosestTarget())
            {
                enemy.ChangeState(enemy.chaseState);
            }
        }
    }

    public override void TargetDeath()
    {
       
    }

    public override void HeardSomething(Vector3 location)
    {
        enemy.targetLastSeenPos = location;
        enemy.ChangeState(enemy.searchState);
    }

    public override void TakeDamage(Vector3 location)
    {
        enemy.targetLastSeenPos = location;
        enemy.ChangeState(enemy.searchState);
    }
}
