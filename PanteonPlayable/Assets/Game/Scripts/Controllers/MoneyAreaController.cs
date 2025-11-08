using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class MoneyAreaController : MonoBehaviour
    {
        [SerializeField, Range(1, 10), Min(1)] private byte columnSize;
        [SerializeField, Range(1, 10), Min(1)] private byte rowSize;
        [SerializeField, Range(0, 10)] private float columnSpace;
        [SerializeField, Range(0, 10)] private float rowSpace;
        [SerializeField, Range(0, 10)] private float heightSpace;
        [SerializeField] private Vector3 moneyRotation;

        [SerializeField] private float moneyMovingToPlayerDuration;
        [SerializeField] private float moneyMovingFromPassengerDuration;

        [SerializeField] private GameObject moneyPrefab;
        [SerializeField] private Transform moneyInitPoint;
        [SerializeField] private byte moneyCountForEachPassenger;
        [SerializeField] private byte eachMoneyValue;
        [SerializeField] private List<GameObject> monies;

        private byte _usedMoneyCount;
        private bool _isTriggeredByPlayer;

        private void Start()
        {
            byte passengerCount = PassengerSignals.Instance.onGetPassengerCount.Invoke();

            byte heightValue = 0;
            byte columnValue = 0;
            byte rowValue = 0;

            for (int i = 0; i < passengerCount * moneyCountForEachPassenger; i++)
            {
                GameObject newMoney = Instantiate(moneyPrefab, transform);
                monies.Add(newMoney);
                newMoney.transform.localPosition = new Vector3(columnValue * columnSpace, heightValue * heightSpace, -rowValue * rowSpace);
                newMoney.transform.localRotation = Quaternion.Euler(moneyRotation);
                newMoney.SetActive(false);

                columnValue++;
                if (columnValue == columnSize)
                {
                    columnValue = 0;
                    rowValue++;

                    if (rowValue == rowSize)
                    {
                        rowValue = 0;
                        heightValue++;
                    }
                }
            }
        }

        public void OnTriggeredByPassenger()
        {
            for (int i = 0; i < moneyCountForEachPassenger; i++)
            {
                GameObject money = monies[_usedMoneyCount];
                Vector3 oldPos = money.transform.position;
                money.transform.position = moneyInitPoint.position;
                money.SetActive(true);
                money.transform.DOJump(oldPos, 4, 1, moneyMovingFromPassengerDuration);
                _usedMoneyCount++;
            }
        }

        public void OnTriggeredByPlayer(Transform triggerController)
        {
            if (_isTriggeredByPlayer) return;

            _isTriggeredByPlayer = true;

            StartCoroutine(MoveMoniesToPlayer(triggerController));
        }

        private IEnumerator MoveMoniesToPlayer(Transform triggerController)
        {
            WaitForSeconds waitSec = new WaitForSeconds(.05f);

            for (int i = 0; i < monies.Count; i++)
            {
                GameObject money = monies[i];

                int i1 = i;

                yield return waitSec;

                money.transform.DOJump(PlayerSignals.Instance.onGetPlayerMoneyPosition.Invoke(), 3, 1, moneyMovingToPlayerDuration)
                    .OnComplete(() =>
                    {
                        money.SetActive(false);
                        EconomySignals.Instance.onAdjustCurrency.Invoke(eachMoneyValue);

                        if (i1 == monies.Count - 1)
                        {
                            triggerController.gameObject.SetActive(false);
                        }
                    });
                money.transform.DOScale(money.transform.localScale / 2, moneyMovingToPlayerDuration / 2).SetDelay(moneyMovingToPlayerDuration / 2);
            }
        }
    }
}