using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERangedAttackState : EnemyState
{
    public Projectile projectilePrefab;
    public float projectileSpeed;
    public Transform[] projectileSpawns;
    public float attackDelay = 0.4f;

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

    void Shoot()
    {
        if (enemy.target == null) return;

        transform.LookAt(enemy.target.position);
        for(int i = 0; i < projectileSpawns.Length;i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawns[i].position, projectileSpawns[i].rotation) as Projectile;
            newProjectile.SetSpeed(projectileSpeed);
            newProjectile.SetDamage(enemy.damage, false);
            newProjectile.transform.LookAt(enemy.target.position + Vector3.up);
        }
    }

    public override IEnumerator ManualUpdate()
    {
        if (isStateActive && enemy.target != null)
        {
            enemy.anim.SetTrigger("Attack");
            transform.LookAt(enemy.target);
            //enemy.target.GetComponent<LivingEntity>().TakeDamage(enemy.damage, false);
            yield return new WaitForSeconds(attackDelay);
            Shoot();

            yield return new WaitForSeconds(1f);
            if (enemy.LookForTarget(2f))
            {
                enemy.ChangeState(enemy.attackState);
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
