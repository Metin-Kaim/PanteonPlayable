using Assets.Game.Scripts.Signals;
using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] short initialCurrency = 50;

    private short _currency;

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

    private void UpdateCurrenyText()
    {
        currencyText.text = _currency.ToString();
    }
}
