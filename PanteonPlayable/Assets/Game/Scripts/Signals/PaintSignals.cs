using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class PaintSignals : MonoBehaviour
    {
        public static PaintSignals Instance;

        public UnityAction<Color> onSetPaintColor;

        private void Awake()
        {
            Instance = this;
        }
    }
}