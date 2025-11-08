using Assets.Game.Scripts.Handlers;
using Assets.Game.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class PaintPanelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI paintPercentText;
        [SerializeField] ColorSelectionButtonHandler[] handlers;

        ColorSelectionButtonHandler _selectedHandler;

        private void OnEnable()
        {
            PaintSignals.Instance.onSetPaintPercent += SetPaintPercentText;
        }
        private void OnDisable()
        {
            PaintSignals.Instance.onSetPaintPercent -= SetPaintPercentText;
        }

        private void Start()
        {
            SetPaintColor(handlers[0]);
        }

        public void SetPaintColor(ColorSelectionButtonHandler selectedHandler)
        {
            if (_selectedHandler == selectedHandler) return;

            if (_selectedHandler != null)
            {
                _selectedHandler.transform.DOKill(true);
                _selectedHandler.transform.DOScale(1, .2f);
            }
            _selectedHandler = selectedHandler;
            _selectedHandler.transform.DOScale(1.1f, .2f);

            PaintSignals.Instance.onSetPaintColor?.Invoke(selectedHandler.SelectionColor);
        }

        public void SetPaintPercentText(string percentText)
        {
            paintPercentText.text = percentText;
        }
    }
}