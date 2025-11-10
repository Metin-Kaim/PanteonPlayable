using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class PlaneSignals : MonoBehaviour
    {
        public static PlaneSignals Instance;

        public UnityAction onIncreasePassengerCount;

        private void Awake()
        {
            Instance = this;
        }
    }
}