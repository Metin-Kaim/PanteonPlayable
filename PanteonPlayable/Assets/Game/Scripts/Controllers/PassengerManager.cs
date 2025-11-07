using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
using Assets.Game.Scripts.Signals;

namespace Assets.Game.Scripts.Controllers
{
    public class PassengerManager : MonoBehaviour
    {
        [SerializeField] private byte passengerCount;
        [SerializeField] private Transform passengerLineStartPoint;
        [SerializeField] private Transform passengerLineStartPoint2;
        [SerializeField] private Vector3 eachPassengerPositionOffset;
        [SerializeField] private Vector3 eachPassengerPositionOffset2;
        [SerializeField] private PassengerMovementController passengerPrefab;

        private List<PassengerMovementController> _passengerList;

        public List<PhasePoint> phasePoints;

        public Vector3 PassengerOffsetWithoutBaggage(PassengerMovementController pass)
        {
            int index = _passengerList.IndexOf(pass);

            return index * eachPassengerPositionOffset2;

        }

        private void Awake()
        {
            _passengerList = new List<PassengerMovementController>();

            for (int i = 0; i < passengerCount; i++)
            {
                PassengerMovementController passenger = Instantiate(passengerPrefab, passengerLineStartPoint.position + i * eachPassengerPositionOffset, Quaternion.Euler(0, 90, 0), transform);
                passenger.phasePoints = new List<PhasePoint>(phasePoints);
                passenger.passengerManager = this;
                _passengerList.Add(passenger);
            }
        }

        public void TriggerEnterWithBaggage(Transform triggerController)
        {
            InputSignals.Instance.onDeactivateInput.Invoke();
            StartCoroutine(IMoveTheLineOfPassengersWithBaggage(triggerController));
        }
        public void TriggerExit()
        {
            InputSignals.Instance.onActivateInput.Invoke();
        }

        public IEnumerator IMoveTheLineOfPassengersWithBaggage(Transform triggerController)
        {
            int passIndex = 0;
            while (passIndex < _passengerList.Count)
            {
                PassengerMovementController firstPass = _passengerList[passIndex];
                firstPass.GiveBaggageToPlayer();

                passIndex++;

                for (int i = 0; i < _passengerList.Count - passIndex; i++)
                {
                    PassengerMovementController pass = _passengerList[i + passIndex];

                    Vector3 targetPos = passengerLineStartPoint.position + i * eachPassengerPositionOffset;

                    pass.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear);
                }

                yield return new WaitForSeconds(.5f);
            }

            triggerController.gameObject.SetActive(false);
        }

        public void MoveTheLineOfPassengersWithoutBaggage(Transform triggerController)
        {
            InputSignals.Instance.onDeactivateInput.Invoke();
            StartCoroutine(IMoveTheLineOfPassengersWithoutBaggage(triggerController));
        }

        public IEnumerator IMoveTheLineOfPassengersWithoutBaggage(Transform triggerController)
        {
            int passIndex = 0;
            while (passIndex < _passengerList.Count)
            {
                PassengerMovementController firstPass = _passengerList[passIndex];
                firstPass.Move();

                passIndex++;

                for (int i = 0; i < _passengerList.Count - passIndex; i++)
                {
                    PassengerMovementController pass = _passengerList[i + passIndex];

                    Vector3 targetPos = passengerLineStartPoint2.position + i * eachPassengerPositionOffset2;

                    pass.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear);
                }

                yield return new WaitForSeconds(.6f);
            }

            triggerController.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class PhasePoint
    {
        public List<Transform> points;
        public float speed;
        public bool join;
        public bool adaptive;
        public bool lookRotation;
        public UnityEvent<PassengerMovementController> itinialAction;
    }
}