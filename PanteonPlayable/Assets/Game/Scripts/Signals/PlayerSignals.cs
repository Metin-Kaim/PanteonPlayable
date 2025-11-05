using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class PlayerSignals : MonoBehaviour
    {
        public static PlayerSignals Instance;

        public Func<Vector3> onGetPlayerPosition;
        public UnityAction<Transform> onSetPlayerParent;
        public UnityAction onResetPlayerParent;
        public UnityAction onClosePlayerCollider;
        public UnityAction onOpenPlayerCollider;
        public Func<Vector3> onGetPlayerBaggagePoint;
        public UnityAction<Transform> onAddBaggage;

        private void Awake()
        {
            Instance = this;
        }
    }
}