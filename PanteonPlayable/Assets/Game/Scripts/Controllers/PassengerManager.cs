using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Controllers
{
    public class PassengerManager : MonoBehaviour
    {
        [SerializeField] private byte passengerCount;
        [SerializeField] private Transform passengerLineStartPoint;
        [SerializeField] private Vector3 eachPassengerPositionOffset;
        [SerializeField] private Vector3 eachPassengerPositionOffset2;
        [SerializeField] private PassengerMovementController passengerPrefab;

        //[SerializeField] private Transform[] pathAfterBaggageCheck;

        private List<PassengerMovementController> _passengerList;
        private List<PassengerMovementController> _passengerListWithBaggage;
        [SerializeField] private List<PassengerMovementController> _passengerListWithoutBaggage;

        public List<PhasePoint> phasePoints;

        public Vector3 PassengerOffsetWithoutBaggage => _passengerListWithoutBaggage.Count * eachPassengerPositionOffset2;

        private void Awake()
        {
            _passengerList = new List<PassengerMovementController>();
            _passengerListWithBaggage = new List<PassengerMovementController>();
            _passengerListWithoutBaggage = new List<PassengerMovementController>();

            for (int i = 0; i < passengerCount; i++)
            {
                PassengerMovementController passenger = Instantiate(passengerPrefab, passengerLineStartPoint.position + i * eachPassengerPositionOffset, Quaternion.Euler(0, 90, 0), transform);
                passenger.phasePoints = new List<PhasePoint>(phasePoints);
                passenger.passengerManager = this;
                _passengerList.Add(passenger);
                _passengerListWithBaggage.Add(passenger);
            }
        }

        public void MoveTheLineOfPassengersWithBaggage()
        {
            if (_passengerListWithBaggage.Count == 0) return;

            StartCoroutine(IMoveTheLineOfPassengersWithBaggage());
        }

        public IEnumerator IMoveTheLineOfPassengersWithBaggage()
        {
            PassengerMovementController firstPass = _passengerListWithBaggage[0];
            _passengerListWithBaggage.Remove(firstPass);
            firstPass.GiveBaggageToPlayer();
            yield return new WaitForSeconds(.3f);

            while (_passengerListWithBaggage.Count > 0)
            {
                for (int i = 0; i < _passengerListWithBaggage.Count; i++)
                {
                    PassengerMovementController pass = _passengerListWithBaggage[i];

                    Vector3 targetPos = passengerLineStartPoint.position + i * eachPassengerPositionOffset;

                    int i1 = i;
                    pass.transform.DOMove(targetPos, .5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        if (i1 == 0)
                        {
                            _passengerListWithBaggage.Remove(pass);
                            pass.GiveBaggageToPlayer();
                        }
                    });

                    //yield return null;
                }

                yield return new WaitForSeconds(1f);
            }
        }

        public void AddObj(PassengerMovementController passengerMovementController)
        {
            _passengerListWithoutBaggage.Add(passengerMovementController);
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