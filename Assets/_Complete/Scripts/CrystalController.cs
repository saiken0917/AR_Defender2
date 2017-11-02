using UnityEngine;
using UnityEngine.UI;
namespace Complete
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider))]
    public class CrystalController : MonoBehaviour
    {
        private const string deadTrigger = "dead";
        private Animator animator;
        private Collider mCollider;
        public float crystalChargeTotalTime = 30f;
        private float curChargeTime;
        public Slider powerSlider;
        public Text progressLabel;
        public ParticleSystem powerEffect;
        public ParticleSystem chargeCompleteEffect;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            mCollider = GetComponent<Collider>();
            powerSlider.maxValue = crystalChargeTotalTime;
        }

        private void OnEnable()
        {
            powerEffect.Play();
        }

        public void OnDead()
        {
            mCollider.enabled = false;
            animator.SetTrigger(deadTrigger);
            powerEffect.Stop();
            enabled = false;
            GameManager.instance.OnCrystalDown();
        }

        private void Update()
        {
            powerSlider.value += Time.deltaTime;
            progressLabel.text = Mathf.FloorToInt(powerSlider.normalizedValue * 100) + "%";
            if (powerSlider.normalizedValue == 1f)
            {
                enabled = false;
                powerEffect.Stop();
                chargeCompleteEffect.Play();
                GameManager.instance.OnCrystalChargeComplete();
            }
        }
    }
}


