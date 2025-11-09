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

        private void OnEnable()
        {
            PassengerSignals.Instance.onGetPassengerCount += () => passengerCount;
        }
        private void OnDisable()
        {
            PassengerSignals.Instance.onGetPassengerCount -= () => passengerCount;
        }

        public void TriggerEnterWithBaggage()
        {
            InputSignals.Instance.onDeactivateInput.Invoke();
            StartCoroutine(IMoveTheLineOfPassengersWithBaggage());
        }
        public void TriggerExit()
        {
            InputSignals.Instance.onActivateInput.Invoke();
        }

        public IEnumerator IMoveTheLineOfPassengersWithBaggage()
        {
            int passIndex = 0;
            while (passIndex < _passengerList.Count)
            {
                PassengerMovementController firstPass = _passengerList[passIndex];
                firstPass.GiveBaggageToPlayer();
                firstPass.animationController.SetCarry(false);
                passIndex++;

                for (int i = 0; i < _passengerList.Count - passIndex; i++)
                {
                    PassengerMovementController pass = _passengerList[i + passIndex];

                    Vector3 targetPos = passengerLineStartPoint.position + i * eachPassengerPositionOffset;

                    pass.animationController.SetRun();
                    pass.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).OnComplete(() => pass.animationController.SetIdle());
                }

                yield return new WaitForSeconds(.5f);
            }

            PlayerSignals.Instance.onSetNextTarget.Invoke();
        }

        public void MoveTheLineOfPassengersWithoutBaggage()
        {
            InputSignals.Instance.onDeactivateInput.Invoke();
            StartCoroutine(IMoveTheLineOfPassengersWithoutBaggage());
        }

        public IEnumerator IMoveTheLineOfPassengersWithoutBaggage()
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

                    pass.animationController.SetRun();
                    pass.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).OnComplete(() => pass.animationController.SetIdle());
                }

                yield return new WaitForSeconds(.6f);
            }

            PlayerSignals.Instance.onSetNextTarget.Invoke();
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
        public bool isRunning;
        public UnityEvent<PassengerMovementController> itinialAction;
    }
}