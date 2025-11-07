using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Signals
{
    public class PassengerSignals : MonoBehaviour
    {
        public static PassengerSignals Instance;

        public Func<byte> onGetPassengerCount;

        private void Awake()
        {
            Instance = this;
        }
    }
}