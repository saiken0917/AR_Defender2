using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
namespace Complete
{
    [RequireComponent(typeof(RagdollBehavior))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class HeroController : MonoBehaviour
    {
        //常數宣告
        private const string enemyTag = "enemy";
        private const string attackBool = "attack";
        //依賴組件宣告
        private RagdollBehavior ragdollBehavior;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private AudioSource audioSource;
        private Collider mCollider;
        //移動轉向速度
        public float turnSmooth = 15f;
        //音效宣告
        public AudioClip fireSound;
        public AudioClip deadSound;
        //攻擊行為相關參數
        public int shootDamage = 5;
        public AttackableBehavior target;
        public RectTransform attackRangeRect;
        public Image attackProbeCircle;
        public Color detectColor;
        public Color normalColor;
        private float attackRadius;
        public LayerMask enemyLayerMask;
        //射擊特效參數
        public ParticleSystem gunShootEffect;

        private void Awake()
        {
            ragdollBehavior = GetComponent<RagdollBehavior>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            mCollider = GetComponent<Collider>();
            attackRadius = attackRangeRect.rect.width / 2;
        }

        //被GameManager呼叫，傳入要移動的目標位置
        public void Move(Vector3 target)
        {
            //如果已死亡，就不做任何事
            if (navMeshAgent.enabled == false)
                return;
            navMeshAgent.SetDestination(target);
            navMeshAgent.isStopped = false;
        }

        private void Update()
        {
            //如果沒有鎖定目標不繼續執行
            if (target == null)
            {
                return;
            }
            //如果沒有在移動，則看向目標並且播放攻擊動畫
            if (navMeshAgent.isStopped)
            {
                animator.SetBool(attackBool, true);
                Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSmooth * Time.deltaTime);
            }
            else
            {
                //移動中取消攻擊行為
                animator.SetBool(attackBool, false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //當碰到敵人直接死亡，並關閉collider避免重複呼叫
            if (other.CompareTag(enemyTag))
            {
                mCollider.enabled = false;
                ragdollBehavior.ToggleRagdoll(true);
                navMeshAgent.enabled = false;
                animator.enabled = false;
                attackProbeCircle.enabled = false;
                enabled = false;
                audioSource.PlayOneShot(deadSound);
            }
        }

        private void FixedUpdate()
        {
            //偵測是否有敵人進入攻擊範圍
            var allCollider = Physics.OverlapSphere(transform.position, attackRadius, enemyLayerMask);
            if (allCollider.Length > 0)
            {
                attackProbeCircle.color = detectColor;
                target = allCollider[0].GetComponent<AttackableBehavior>();
            }
            else
            {
                //如果範圍內沒有敵人，將Target設為空值
                attackProbeCircle.color = normalColor;
                target = null;
                animator.SetBool(attackBool, false);
            }
        }

        //被動畫呼叫
        public void OnGunTrigger()
        {
            //有可能目標已經被刪除，故需要做防呆
            if (target == null)
                return;
            gunShootEffect.Play();
            audioSource.PlayOneShot(fireSound);
            target.Hurt(shootDamage);
        }
    }

}
