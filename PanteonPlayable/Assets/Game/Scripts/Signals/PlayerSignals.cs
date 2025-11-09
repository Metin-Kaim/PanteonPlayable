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
        public UnityAction onClosePlayerCollider;
        public UnityAction onOpenPlayerCollider;
        public Func<Vector3> onGetPlayerBaggagePoint;
        public UnityAction<Transform> onAddBaggage;
        public Func<List<Transform>> onGetAllBaggages;
        public Func<Vector3> onGetPlayerMoneyPosition;
        public Func<Transform> onGetPlayer;
        public UnityAction onSetNextTarget;
        public UnityAction onCloseNavigation;
        public UnityAction onOpenNavigation;
        public UnityAction onSetRunAnimation;
        public UnityAction onSetIdleAnimation;

        private void Awake()
        {
            Instance = this;
        }
    }
}