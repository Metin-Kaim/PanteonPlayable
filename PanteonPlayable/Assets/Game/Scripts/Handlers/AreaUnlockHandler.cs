using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System;
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
        [SerializeField] private GameObject[] objectsToOpen;
        [SerializeField] private GameObject[] objectsToOpenWithDelay;
        [SerializeField] private float delay;
        [SerializeField] private float openingDuration;
        [SerializeField] private float delayedOpeningDuration;
        [SerializeField] private bool cameraBackToBase;
        [SerializeField] private bool isOpened;
        [SerializeField] private GameObject placeArrow;

        private short _currentCurrencyCost;
        private short _fillCycleCount = 0;
        private Vector3 initScale;

        private void OnEnable()
        {
            EconomySignals.Instance.onAdjustedCurrency += GetCheckAvailability;
        }

        private void GetCheckAvailability(short currency)
        {
            if (isOpened) return;

            if (currencyCost <= currency)
            {
                OpenObject();
            }
        }

        private void OpenObject()
        {
            isOpened = true;

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            DOTween.To(() => canvasGroup.alpha, value => canvasGroup.alpha = value, 1, .3f).SetEase(Ease.Linear);
            transform.DOScale(initScale * 1.1f, 0.3f).SetEase(Ease.OutBack);

            placeArrow.SetActive(true);
        }

        private void OnDisable()
        {
            EconomySignals.Instance.onAdjustedCurrency -= GetCheckAvailability;
        }

        private void Start()
        {
            _currentCurrencyCost = currencyCost;
            UpdateCurrencyCostText();
            initScale = transform.localScale;
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
                item.transform.DOScale(0, delayedOpeningDuration).From().SetEase(Ease.Linear).SetDelay(openingDuration + delay * i).OnComplete(() =>
                {
                    if (cameraBackToBase)
                        if (i1 == objectsToOpenWithDelay.Length - 1)
                        {
                            CameraSignals.Instance.onBackToBase.Invoke();
                        }
                });
            }
        }

        public void TriggerEnter()
        {
            InputSignals.Instance.onDeactivateInput.Invoke();

            StartCoroutine(FillTheBar());
        }

        private IEnumerator FillTheBar()
        {
            while (fillBar.fillAmount < 1f)
            {
                _fillCycleCount++;

                fillBar.fillAmount += 1f / currencyCost;

                _currentCurrencyCost--;

                if (_currentCurrencyCost < 0) _currentCurrencyCost = 0;

                EconomySignals.Instance.onAdjustCurrency.Invoke(-1);
                UpdateCurrencyCostText();

                if (_fillCycleCount >= currencyCost) break;

                yield return new WaitForSeconds(fillDuration);
            }

            OnBarFilled();
        }

        private void OnBarFilled()
        {
            fillBar.fillAmount = 1;

            UnlockTheObjects();

            transform.DOScale(0, closeThisObjectDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}