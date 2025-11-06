using System;
using System.Collections;
using System.Collections.Generic;
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
        public Func<List<Transform>> onGetAllBaggages;

        private void Awake()
        {
            Instance = this;
        }
    }
}