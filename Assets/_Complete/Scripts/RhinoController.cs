using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
namespace Complete
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class RhinoController : MonoBehaviour
    {
        //常數宣告
        private const string roarTrigger = "roar";
        private const string crystalTag = "crystal";
        private const string deadTrigger = "dead";
        //衝撞攻擊行為參數
        public float impactChargeTime = 1f;
        public Transform impactTarget;
        public float impactSpeed = 10f;
        private float impactDistance;
        public int impactDamage = 50;
        public ParticleSystem explosionEffect;
        public float walkingSpeed = 3f;
        public Slider aimSlider;
        //音效宣告
        public AudioClip roarSound;
        public AudioClip deadSound;
        public AudioClip impactSound;
        //依賴組件宣告
        private Animator animator;
        private AudioSource audioSource;
        private NavMeshAgent navMeshAgent;
        //內部狀態暫存變數
        private bool isImpacting;
        private Transform target;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            impactDistance = Vector3.Distance(impactTarget.position, transform.position);
        }

        private void OnEnable()
        {
            var go = GameObject.FindGameObjectWithTag(crystalTag);
            if (go != null)
            {
                target = go.transform;
                StartCoroutine("ProcessState");
            }
        }

        IEnumerator ProcessState()
        {
            while (target != null)
            {
                navMeshAgent.speed = walkingSpeed;
                var randomRad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                var randomPos = target.position + new Vector3(Mathf.Cos(randomRad), 0, Mathf.Sin(randomRad)) * impactDistance;
                navMeshAgent.SetDestination(randomPos);
                navMeshAgent.isStopped = false;
                //設定完目的地後，需等下一幀再開始判斷是否抵達
                yield return null;
                while (navMeshAgent.isStopped == false)
                    yield return null;
                //如果目標已消失，取消攻擊
                if (target == null)
                    yield break;
                //衝擊前，先朝向目標
                transform.LookAt(target);
                yield return StartCoroutine("ProcessImpact");
                yield return null;
                while (navMeshAgent.isStopped == false)
                    yield return null;
                isImpacting = false;
                yield return new WaitForSeconds(2f);
            }
        }

        IEnumerator ProcessImpact()
        {
            aimSlider.gameObject.SetActive(true);
            aimSlider.value = aimSlider.minValue;
            aimSlider.maxValue = impactChargeTime;
            animator.SetTrigger(roarTrigger);
            while (aimSlider.value < aimSlider.maxValue)
            {
                aimSlider.value += Time.deltaTime;
                yield return null;
            }
            aimSlider.gameObject.SetActive(false);
            navMeshAgent.isStopped = false;
            Vector3 targetPos = impactTarget.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, impactDistance))
            {
                if (hit.collider.CompareTag(crystalTag))
                {
                    targetPos = hit.point;
                }
            }
            navMeshAgent.speed = impactSpeed;
            navMeshAgent.SetDestination(targetPos);
            isImpacting = true;
        }
        //被動畫事件呼叫
        public void TriggerRoarSound()
        {
            audioSource.PlayOneShot(roarSound);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(crystalTag))
            {
                if (isImpacting)
                {
                    isImpacting = false;
                    explosionEffect.Play();
                    audioSource.PlayOneShot(impactSound);
                    var attackableObj = other.GetComponent<AttackableBehavior>();
                    if (attackableObj != null)
                        attackableObj.Hurt(impactDamage);
                }
            }
            else
            {
                explosionEffect.Play();
            }
        }
        //被AttackableBehavior呼叫
        public void OnDead()
        {
            StopCoroutine("ProcessState");
            StopCoroutine("ProcessImpact");
            navMeshAgent.enabled = false;
            audioSource.PlayOneShot(deadSound);
            animator.SetTrigger(deadTrigger);
        }
    }
}
