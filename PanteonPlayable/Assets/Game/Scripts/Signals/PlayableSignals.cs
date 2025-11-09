using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Signals
{
    public class PlayableSignals : MonoBehaviour
    {
        public static PlayableSignals Instance;

        public UnityAction onGoToStore;

        private void Awake()
        {
            Instance = this;
        }
    }
}