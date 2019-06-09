using UnityEngine;
using System.Collections;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyStateManager enemy;
    protected bool isStateActive = false;

    //public EnemyState(EnemyStageManager _enemy)
    //{
    //    enemy = _enemy;
    //}

    protected virtual void Start()
    {
        enemy = gameObject.GetComponent<EnemyStateManager>();
    }

    abstract public void EnterState();

    abstract public void ExitState();

    abstract public IEnumerator ManualUpdate();

    abstract public void UpdateState();

    abstract public void TargetDeath();

    abstract public void TakeDamage(Vector3 originPostion);

    abstract public void HeardSomething(Vector3 location);
}
