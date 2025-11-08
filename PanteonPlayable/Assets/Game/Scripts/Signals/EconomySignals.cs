using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class EconomySignals : MonoBehaviour
    {
        public static EconomySignals Instance;

        public UnityAction<short> onAdjustCurrency;
        public UnityAction<short> onAdjustedCurrency;

        private void Awake()
        {
            Instance = this;
        }
    }
}