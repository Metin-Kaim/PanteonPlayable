using Assets.Game.Scripts.Handlers;
using Assets.Game.Scripts.Signals;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class ColorSelectionButtonController : MonoBehaviour
    {
        [SerializeField] ColorSelectionButtonHandler[] handlers;



        public void SetPaintColor(ColorSelectionButtonHandler selectionHandler)
        {
            foreach (var handler in handlers)
            {
                if (handler == selectionHandler)
                    selectionHandler.GetComponent<RectTransform>().transform.localScale *= 1.1f;
                else
                    selectionHandler.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            }
            PaintSignals.Instance.onSetPaintColor?.Invoke(selectionHandler.SelectionColor);
        }
    }
}