using UnityEngine;
namespace Complete
{
    public class LookAtCamera : MonoBehaviour
    {
        public Vector3 scalePos;
        private void Update()
        {
            var target = Camera.main.transform.position;
            target = Vector3.Scale(target, scalePos);
            transform.LookAt(target);
        }
    }

}
