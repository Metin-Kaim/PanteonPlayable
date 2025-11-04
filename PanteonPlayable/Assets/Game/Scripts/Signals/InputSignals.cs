using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class InputSignals : MonoBehaviour
    {
        public static InputSignals Instance;

        public Func<Vector2> onGetInput;
        public UnityAction onActivateInput;
        public UnityAction onDeactivateInput;

        private void Awake()
        {
            Instance = this;
        }
    }
}