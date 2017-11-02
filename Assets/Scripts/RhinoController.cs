using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class RhinoController : MonoBehaviour
{
    //常數宣告
    private string roarTrigger = "roar";
    private string crystalTag = "crystal";
    private string deadTrigger = "dead";
    //衝撞攻擊行為參數
    public float impactChargeTime = 1f;
    public Transform impactTarget;
    public float impactSpeed = 10f;
    public int impactDamage = 50;
    public ParticleSystem explosionEffect;
    public float walkingSpeed = 3f;
    public Slider aimSlider;
    //音效宣告
    public AudioClip roarSound;
    public AudioClip deadSound;
    public AudioClip impactSound;
    //依賴的組件宣告
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;
    private Animator animator;

    public float hitOffset = 1;
    public Transform target;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StartCoroutine("ProcessImpact");
    }

    private IEnumerator ProcessImpact()
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
        navMeshAgent.speed = impactSpeed;
        RaycastHit hit;
        float distance = Vector3.Distance(impactTarget.position, transform.position);
        Vector3 targetPos = impactTarget.position;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if(hit.collider.CompareTag(crystalTag))
            {
                targetPos = hit.point;
            }
        }
        navMeshAgent.SetDestination(targetPos - transform.forward * hitOffset);
    }
    private void Update()
    {
        //navMeshAgent.SetDestination(target.position);
    }

    public void TriggerRoarSound()
    {
        audioSource.PlayOneShot(roarSound);
    }
}
