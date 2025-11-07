using Assets.Game.Scripts.Abstract;
using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Handlers
{
    public class AreaUnlockHandler : MonoBehaviour
    {
        [Header("Fill Bar Settings")]
        [SerializeField] private Image fillBar;
        [SerializeField] private TextMeshProUGUI currencyCostText;
        [SerializeField] private short currencyCost;
        [SerializeField] private float fillDuration;
        [SerializeField] private float closeThisObjectDuration;

        [Header("Unlock Settings")]
        [SerializeField] private TriggerController triggerController;
        [SerializeField] private GameObject[] objectsToOpen;
        [SerializeField] private GameObject[] objectsToOpenWithDelay;
        [SerializeField] private float delay;
        [SerializeField] private float openingDuration;

        private Coroutine fillBarCoroutine;
        private short _currentCurrencyCost;
        private short _fillCycleCount = 0;

        private void Start()
        {
            _currentCurrencyCost = currencyCost;
            UpdateCurrencyCostText();
        }

        private void UpdateCurrencyCostText()
        {
            currencyCostText.text = _currentCurrencyCost.ToString();
        }

        public void UnlockTheObjects()
        {
            foreach (var item in objectsToOpen)
            {
                item.SetActive(true);
                item.transform.DOScale(0, openingDuration).From().SetEase(Ease.Linear);
            }

            for (int i = 0; i < objectsToOpenWithDelay.Length; i++)
            {
                GameObject item = objectsToOpenWithDelay[i];
                item.SetActive(true);

                int i1 = i;
                item.transform.DOScale(0, openingDuration).From().SetEase(Ease.Linear).SetDelay(openingDuration + delay * i).OnComplete(() =>
                {
                    if (i1 == objectsToOpenWithDelay.Length - 1)
                    {
                        CameraSignals.Instance.onBackToBase.Invoke();
                    }
                });
            }
        }

        public void TriggerEnter()
        {
            fillBarCoroutine = StartCoroutine(FillTheBar());
        }

        public void TriggerExit()
        {
            if (_fillCycleCount >= currencyCost) return;

            if (fillBarCoroutine != null)
            {
                StopCoroutine(fillBarCoroutine);
            }
        }

        private IEnumerator FillTheBar()
        {
            while (fillBar.fillAmount < 1f)
            {
                _fillCycleCount++;

                fillBar.fillAmount += 1f / currencyCost;

                _currentCurrencyCost--;

                if (_currentCurrencyCost < 0) _currentCurrencyCost = 0;

                CanvasSignals.Instance.onAdjustCurrency.Invoke(-1);
                UpdateCurrencyCostText();

                if (_fillCycleCount >= currencyCost) break;

                yield return new WaitForSeconds(fillDuration);
            }
            fillBar.fillAmount = 1;
            fillBarCoroutine = null;

            UnlockTheObjects();

            CameraSignals.Instance.onMoveToTarget.Invoke();
            InputSignals.Instance.onDeactivateInput.Invoke();

            triggerController.gameObject.SetActive(false);

            transform.DOScale(0, closeThisObjectDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}