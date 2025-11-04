using Assets.Game.Scripts.Abstract;
using Assets.Game.Scripts.Signals;
using System.Collections;
using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour/*, ITrigger*/
{
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] short initialCurrency = 50;

    private short _currency;

    private Coroutine currencyCoroutine;


    private void Start()
    {
        _currency = initialCurrency;
        UpdateCurrenyText();
    }

    private void OnEnable()
    {
        CanvasSignals.Instance.onAdjustCurrency += AdjustCurreny;
    }
    private void OnDisable()
    {
        CanvasSignals.Instance.onAdjustCurrency -= AdjustCurreny;
    }

    private void AdjustCurreny(short value)
    {
        _currency += value;

        if (_currency < 0) _currency = 0;

        UpdateCurrenyText();
    }

    //private IEnumerator AdjustCurrencyByTime()
    //{
    //    _currency--;
    //    UpdateCurrenyText();

    //    currencyCoroutine = null;
    //}

    //public void TriggerEnter()
    //{
    //    currencyCoroutine = StartCoroutine(AdjustCurrencyByTime());
    //}

    //public void TriggerExit()
    //{
    //    if (currencyCoroutine != null)
    //    {
    //        StopCoroutine(currencyCoroutine);
    //    }
    //}
    private void UpdateCurrenyText()
    {
        currencyText.text = _currency.ToString();
    }
}
