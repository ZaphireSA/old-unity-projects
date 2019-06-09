using UnityEngine;
using System.Collections;
using System;

public class ESearchState : EnemyState
{
    public override void EnterState()
    {
        isStateActive = true;
        enemy.meshRendererFlag.material.color = Color.yellow;

        enemy.navAgent.enabled = true;
        enemy.navAgent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("SpeedPercent", 1f);

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
            float distToTarget = (enemy.targetLastSeenPos - enemy.transform.position).magnitude;
            enemy.navAgent.SetDestination(enemy.targetLastSeenPos);
            enemy.navAgent.Resume();

            while (enemy.navAgent != null && enemy.navAgent.remainingDistance > 0.3f) //Arrived.
            {
                yield return null;
            }

            enemy.ChangeState(enemy.roamState);

            //yield return new WaitForSeconds(Random.Range(1, enemy.maxRoamWait));
            yield return null;
        }
    }

    public override void UpdateState()
    {        
        if (enemy.isRaged)
        {
            if (enemy.GetClosestTarget())
            {
                enemy.ChangeState(enemy.chaseState);
            }
        } else if (enemy.LookForTarget())
        {
            enemy.ChangeState(enemy.chaseState);
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
