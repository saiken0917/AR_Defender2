using UnityEngine;
using Vuforia;
namespace Complete
{
    public class MoveTargetBehavior : MonoBehaviour, ITrackableEventHandler
    {
        private Vector3 lastPos;
        public float moveThreshold = 0.5f;

        void Start()
        {
            var mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                   newStatus == TrackableBehaviour.Status.TRACKED ||
                   newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, lastPos) >= moveThreshold)
            {
                lastPos = transform.position;
                GameManager.instance.BroadcastMovePos(transform.position);
            }
        }
    }
}

