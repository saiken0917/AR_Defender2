using UnityEngine;
using Vuforia;
namespace Complete
{
    public class BattleFieldController : MonoBehaviour, ITrackableEventHandler
    {
        private float lostTime;
        public float maxLostTime = 1f;
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
                enabled = false;
                GameManager.instance.BattleFieldReady();
            }
            else
            {
                lostTime = 0;
                enabled = true;
            }
        }

        private void Update()
        {
            lostTime += Time.unscaledDeltaTime;
            if (lostTime >= maxLostTime)
            {
                enabled = false;
                GameManager.instance.BattleFieldLost();
            }
        }
    }
}

