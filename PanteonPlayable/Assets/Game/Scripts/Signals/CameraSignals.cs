using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class CameraSignals : MonoBehaviour
    {
        public static CameraSignals Instance;

        public UnityAction<Transform> onMoveToTarget;
        public UnityAction onBackToBase;

        private void Awake()
        {
            Instance = this;
        }
    }
}