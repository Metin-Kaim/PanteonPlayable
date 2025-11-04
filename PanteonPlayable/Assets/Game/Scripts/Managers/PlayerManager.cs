using Assets.Game.Scripts.Signals;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerSignals.Instance.onGetPlayerPosition += OnGetPlayerPosition;
        }

        private Vector3 OnGetPlayerPosition()
        {
            return transform.position;
        }
        private void OnDisable()
        {
            PlayerSignals.Instance.onGetPlayerPosition -= OnGetPlayerPosition;
        }
    }
}