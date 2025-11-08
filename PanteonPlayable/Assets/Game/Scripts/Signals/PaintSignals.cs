using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class PaintSignals : MonoBehaviour
    {
        public static PaintSignals Instance;

        public UnityAction<Color> onSetPaintColor;
        public UnityAction<string> onSetPaintPercent;

        private void Awake()
        {
            Instance = this;
        }
    }
}