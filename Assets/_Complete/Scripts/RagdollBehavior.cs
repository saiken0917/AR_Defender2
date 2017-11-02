using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Complete
{
    public class RagdollBehavior : MonoBehaviour
    {

        private Rigidbody[] allRigids;
        public Transform root;
        private void Awake()
        {
            allRigids = root.GetComponentsInChildren<Rigidbody>();
            ToggleRagdoll(false);
        }

        public void ToggleRagdoll(bool toggle)
        {
            for (int i = 0; i < allRigids.Length; i++)
            {
                allRigids[i].isKinematic = !toggle;
            }
        }
    }

}
