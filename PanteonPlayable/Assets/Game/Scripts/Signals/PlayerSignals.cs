using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Signals
{
    public class PlayerSignals : MonoBehaviour
    {
        public static PlayerSignals Instance;

        public Func<Vector3> onGetPlayerPosition;

        private void Awake()
        {
            Instance = this;
        }
    }
}