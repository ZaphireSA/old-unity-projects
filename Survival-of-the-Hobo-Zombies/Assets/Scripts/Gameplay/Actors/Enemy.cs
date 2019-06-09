using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Roaming, Chasing, Spawning, Attacking, Search }
    State currentState;

    public enum AttackType { Melee, Ranged }
    public AttackType attackType = AttackType.Melee;

    UnityEngine.AI.NavMeshAgent pathFinder;
    Transform target;

    void Senses() { }

    void Idle() { }

    void Roaming() { }

    void Chasing() { }

    void Search() { }

    void Spawning() { }

    void Attacking() { }
}
