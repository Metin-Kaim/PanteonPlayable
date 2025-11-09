using Assets.Game.Scripts.Handlers;
using Assets.Game.Scripts.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Collider playerCollider;
        [SerializeField] private Transform baggagePoint;
        [SerializeField] private Vector3 baggageOffset;
        [SerializeField] private List<Transform> baggages = new List<Transform>();
        [SerializeField] private PlayerNavigatorController navigatorController;

        private void OnEnable()
        {
            PlayerSignals.Instance.onGetPlayerPosition += OnGetPlayerPosition;
            PlayerSignals.Instance.onGetPlayerMoneyPosition += OnGetPlayerMoneyPoint;
            PlayerSignals.Instance.onClosePlayerCollider += OnClosePlayerCollider;
            PlayerSignals.Instance.onOpenPlayerCollider += OnOpenPlayerCollider;
            PlayerSignals.Instance.onGetPlayerBaggagePoint += OnGetPlayerBaggagePoint;
            PlayerSignals.Instance.onAddBaggage += OnAddBaggage;
            PlayerSignals.Instance.onGetAllBaggages += OnGetAllBaggages;
            PlayerSignals.Instance.onGetPlayer += () => transform;
            PlayerSignals.Instance.onSetNextTarget += navigatorController.SetNextTarget;
            PlayerSignals.Instance.onCloseNavigation += navigatorController.CloseNavigation;
            PlayerSignals.Instance.onOpenNavigation += navigatorController.OpenNavigation;
        }

        private Vector3 OnGetPlayerMoneyPoint()
        {
            return transform.position + transform.forward * .5f + transform.up * 0.5f;
        }

        private List<Transform> OnGetAllBaggages()
        {
            List<Transform> newBaggages = baggages.ToList();
            baggages.Clear();
            return newBaggages;
        }

        private void OnAddBaggage(Transform baggage)
        {
            if (baggages.Contains(baggage)) return;

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

        private Vector3 OnGetPlayerPosition()
        {
            return transform.position;
        }
        private void OnDisable()
        {
            PlayerSignals.Instance.onGetPlayerPosition -= OnGetPlayerPosition;
            PlayerSignals.Instance.onGetPlayerMoneyPosition += OnGetPlayerMoneyPoint;
            PlayerSignals.Instance.onClosePlayerCollider -= OnClosePlayerCollider;
            PlayerSignals.Instance.onOpenPlayerCollider -= OnOpenPlayerCollider;
            PlayerSignals.Instance.onGetPlayerBaggagePoint -= OnGetPlayerBaggagePoint;
            PlayerSignals.Instance.onAddBaggage -= OnAddBaggage;
            PlayerSignals.Instance.onGetAllBaggages -= OnGetAllBaggages;
            PlayerSignals.Instance.onGetPlayer -= () => transform;
        }
    }
}