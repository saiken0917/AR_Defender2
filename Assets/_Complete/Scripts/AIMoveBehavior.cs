using UnityEngine;
using UnityEngine.AI;
namespace Complete
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMoveBehavior : MonoBehaviour
    {
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        public string animSpeedParam = "speed";
        public float speedAnimRatio = 0.2f;
        public float turnSmooth = 15f;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            if (navMeshAgent.enabled == false)
            {
                animator.SetFloat(animSpeedParam, 0);
                return;
            }
            float speed = navMeshAgent.desiredVelocity.sqrMagnitude;
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                speed = 0f;
                navMeshAgent.isStopped = true;
            }
            else if (speed != 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSmooth * Time.deltaTime);
            }
            animator.SetFloat(animSpeedParam, speed * speedAnimRatio);
        }
    }
}

