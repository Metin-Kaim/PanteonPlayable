using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class CanvasSignals : MonoBehaviour
    {
        public static CanvasSignals Instance;

        public UnityAction<short> onAdjustCurrency;

        private void Awake()
        {
            Instance = this;
        }
    }
}