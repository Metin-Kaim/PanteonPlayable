using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Signals
{
    public class DataSignals : MonoBehaviour
    {
        public static DataSignals Instance;

        public Func<Vector2> onGetJoystickDirection;

        private void Awake()
        {
            Instance = this;
        }
    }
}