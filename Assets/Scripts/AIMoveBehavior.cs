using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMoveBehavior : MonoBehaviour {
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    public string animSpeedParam = "speed";
    public float turnSmooth = 15f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        float speed = navMeshAgent.desiredVelocity.sqrMagnitude;
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            speed = 0f;
            //navMeshAgent.isStopped = true;
        }
        else if(speed != 0f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSmooth * Time.deltaTime);
        }
        animator.SetFloat(animSpeedParam, speed);
    }
}
