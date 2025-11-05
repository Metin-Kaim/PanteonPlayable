using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Collider playerCollider;
        [SerializeField] private Transform baggagePoint;
        [SerializeField] private Vector3 baggageOffset;
        [SerializeField] private List<Transform> baggages = new List<Transform>();

        private Transform _initialParent;

        private void Start()
        {
            _initialParent = transform.parent;
        }

        private void OnEnable()
        {
            PlayerSignals.Instance.onGetPlayerPosition += OnGetPlayerPosition;
            PlayerSignals.Instance.onSetPlayerParent += OnSetPlayerParent;
            PlayerSignals.Instance.onResetPlayerParent += OnResetPlayerParent;
            PlayerSignals.Instance.onClosePlayerCollider += OnClosePlayerCollider;
            PlayerSignals.Instance.onOpenPlayerCollider += OnOpenPlayerCollider;
            PlayerSignals.Instance.onGetPlayerBaggagePoint += OnGetPlayerBaggagePoint;
            PlayerSignals.Instance.onAddBaggage += OnAddBaggage;
        }

        private void OnAddBaggage(Transform baggage)
        {
            baggage.SetParent(baggagePoint.transform);
            baggages.Add(baggage);
        }

        private Vector3 OnGetPlayerBaggagePoint()
        {
            return baggageOffset * baggages.Count;
        }

        private void OnOpenPlayerCollider()
        {
            playerCollider.enabled = true;
        }

        private void OnClosePlayerCollider()
        {
            playerCollider.enabled = false;
        }

        private void OnResetPlayerParent()
        {
            transform.SetParent(_initialParent);
        }

        private void OnSetPlayerParent(Transform newParent)
        {
            transform.SetParent(newParent);
            transform.DOLocalMove(Vector3.zero, .2f).SetEase(Ease.Linear);
        }

        private Vector3 OnGetPlayerPosition()
        {
            return transform.position;
        }
        private void OnDisable()
        {
            PlayerSignals.Instance.onGetPlayerPosition -= OnGetPlayerPosition;
            PlayerSignals.Instance.onSetPlayerParent -= OnSetPlayerParent;
            PlayerSignals.Instance.onResetPlayerParent -= OnResetPlayerParent;
            PlayerSignals.Instance.onClosePlayerCollider -= OnClosePlayerCollider;
            PlayerSignals.Instance.onOpenPlayerCollider -= OnOpenPlayerCollider;
            PlayerSignals.Instance.onGetPlayerBaggagePoint -= OnGetPlayerBaggagePoint;
            PlayerSignals.Instance.onAddBaggage -= OnAddBaggage;
        }
    }
}