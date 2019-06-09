using UnityEngine;
using System.Collections;
using System;

public class EAttackState : EnemyState
{

    public override void EnterState()
    {
        isStateActive = true;
        enemy.meshRendererFlag.material.color = Color.red;

        enemy.navAgent.enabled = true;
        enemy.navAgent.speed = 0;
        enemy.anim.SetFloat("SpeedPercent", 0);

        StartCoroutine(ManualUpdate());
    }

    public override void ExitState()
    {
        isStateActive = false;
        StopAllCoroutines();
    }

    public override IEnumerator ManualUpdate()
    {
        if (isStateActive && enemy.target != null)
        {
            enemy.anim.SetTrigger("Attack");
            enemy.target.GetComponent<LivingEntity>().TakeDamage(enemy.damage, false);
            yield return new WaitForSeconds(1f);
            if (!enemy.isRaged)
            {
                if (enemy.LookForTarget())
                {
                    enemy.ChangeState(enemy.chaseState);
                }
                else
                {
                    if (enemy.target != null)
                    {
                        enemy.targetLastSeenPos = enemy.target.position;
                        enemy.target = null;
                        enemy.ChangeState(enemy.searchState);
                    }
                    else
                    {
                        enemy.ChangeState(enemy.roamState);
                    }
                }
            } else
            {
                if (enemy.target == null)
                {
                    enemy.GetClosestTarget();
                }
                enemy.ChangeState(enemy.chaseState);
            }
        }
        yield return null;
    }   

    public override void UpdateState()
    {

    }

    public override void TargetDeath()
    {

    }

    public override void HeardSomething(Vector3 location)
    {

    }

    public override void TakeDamage(Vector3 originPostion)
    {

    }
}
