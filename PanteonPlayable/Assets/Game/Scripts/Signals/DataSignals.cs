using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Signals
{
    public class DataSignals : MonoBehaviour
    {
        public static DataSignals Instance;

        public Func<Material> onGetRandomColorMaterial;

        private void Awake()
        {
            Instance = this;
        }
    }
}