using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerAI : MonoBehaviour {

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    float walkSpeed = 5f;
    [SerializeField]
    float runSpeed = 10f;
    Vector3 targetPos;

    Animator anim;

    public
    Renderer rend;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("Type", 2);
    }

    private void Start()
    {
        
        targetPos = transform.position;
        agent.speed = walkSpeed;
        SetTarget(transform.position);
        
        //StartCoroutine(Roam());
    }

    public void SetTarget(Vector3 pos)
    {
        //Debug.DrawLine(fleePoint, fleePoint + new Vector3(0, 5, 0),Color.blue, 3f);
        NavMeshHit hit;
        NavMesh.SamplePosition(pos, out hit, 3, 1);
        targetPos = hit.position;

        //targetPos = RandomNavSphere(transform.position, maxRoamDistance, 1);
        agent.SetDestination(targetPos);

        anim.SetFloat("SpeedZ", agent.velocity.magnitude); 
    }

}
