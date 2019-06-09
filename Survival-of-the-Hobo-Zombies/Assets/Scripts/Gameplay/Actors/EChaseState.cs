using UnityEngine;
using System.Collections;

public class EChaseState : EnemyState
{
    protected override void Start()
    {
        base.Start();
    }

    public override void EnterState()
    {
        isStateActive = true;
        enemy.meshRendererFlag.material.color = Color.green;

        enemy.navAgent.enabled = true;
        enemy.navAgent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("SpeedPercent", 1f);

        if (enemy.audioSource.clip != enemy.audioChase)
        {
            enemy.audioSource.clip = enemy.audioChase;
            enemy.audioSource.Play();
        }

        StartCoroutine(ManualUpdate());
        StartCoroutine(SearchForPlayer());
    }

    public override void ExitState()
    {
        isStateActive = false;        
        StopAllCoroutines();
    }

    public override IEnumerator ManualUpdate()
    {
        while (isStateActive && enemy.target != null)
        {
            float distToTarget = (enemy.target.position - enemy.transform.position).magnitude;
            enemy.navAgent.SetDestination(enemy.target.position);
            enemy.navAgent.Resume();            

            if (enemy.navAgent.remainingDistance < Mathf.Pow(enemy.attackDistanceThreshold + enemy.myCollisionRadius + enemy.targetColissionRadius,2)) //Arrived.
            {
                Vector3 direction = enemy.target.transform.position - enemy.eyesPoint.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(enemy.eyesPoint.transform.position, direction.normalized, out hit, enemy.fovRange, enemy.visionMask) && hit.collider.CompareTag("Player"))
                {
                    enemy.ChangeState(enemy.attackState);
                }
                
            }

            //yield return new WaitForSeconds(Random.Range(1, enemy.maxRoamWait));
            yield return null;
        }
    }

    IEnumerator SearchForPlayer()
    {
        while (isStateActive && enemy.target != null)
        {
            if (!enemy.LookForTarget() && !enemy.isRaged)
            {
                enemy.targetLastSeenPos = enemy.target.position;
                enemy.target = null;
                enemy.ChangeState(enemy.searchState);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
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
