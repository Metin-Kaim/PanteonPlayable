using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Handlers
{
    public class PlaneHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshPro passengerText;

        private int _count = 0;
        private int _maxPassengers;

        private void OnEnable()
        {
            PlaneSignals.Instance.onIncreasePassengerCount += IncreasePassengerCount;
        }
        private void OnDestroy()
        {
            PlaneSignals.Instance.onIncreasePassengerCount -= IncreasePassengerCount;
        }

        private void Start()
        {
            _maxPassengers = PassengerSignals.Instance.onGetPassengerCount.Invoke();
        }

        private void IncreasePassengerCount()
        {
            _count++;
            passengerText.text = _count + "/" + _maxPassengers;

            if (_count >= _maxPassengers)
            {
                Move();
            }
        }

        private void Move()
        {
            transform.DOMoveZ(-15, 2).SetRelative(true).SetEase(Ease.InSine).OnComplete(() => gameObject.SetActive(false));
        }
    }
}