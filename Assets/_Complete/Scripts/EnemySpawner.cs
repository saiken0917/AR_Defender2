using System.Collections;
using UnityEngine;
namespace Complete
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform[] spawnPoints;
        public GameObject enemy;
        public float delayStartTime = 3f;
        public float intervalTime = 5f;
        public void OnEnable()
        {
            StartCoroutine("ProcessSpawn");
        }

        public void OnDisable()
        {
            StopCoroutine("ProcessSpawn");
        }

        private IEnumerator ProcessSpawn()
        {
            yield return new WaitForSeconds(delayStartTime);
            while (true)
            {
                var pointIndex = Random.Range(0, spawnPoints.Length - 1);
                Instantiate(enemy, spawnPoints[pointIndex].position, Quaternion.identity);
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }
}

