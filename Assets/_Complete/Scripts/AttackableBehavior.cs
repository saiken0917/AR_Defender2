using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Complete
{
    public class AttackableBehavior : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;
        public float smooth = 5f;
        public float delayDestroy = 1.5f;
        public Slider healthSlider;
        public UnityEvent onDead;
        public UnityEvent onHurt;
        private void OnEnable()
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = healthSlider.maxValue;
            currentHealth = (int)healthSlider.maxValue;
        }

        public void Hurt(int damage)
        {
            if (currentHealth <= healthSlider.minValue)
            {
                return;
            }
            currentHealth -= damage;
            onHurt.Invoke();
            if (currentHealth <= healthSlider.minValue)
            {
                onDead.Invoke();
                Destroy(gameObject, delayDestroy);
            }
        }

        private void Update()
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * smooth);
        }
    }
}

