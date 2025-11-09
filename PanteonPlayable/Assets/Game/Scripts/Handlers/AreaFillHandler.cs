using DG.Tweening;
using UnityEngine;

namespace Assets.Game.Scripts.Handlers
{
    public class AreaFillHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color openingColor;
        [SerializeField] private float scaleMultiplier;
        [SerializeField] private float animationDuration;

        private Vector3 initScale;
        private bool isOpened;

        private void Start()
        {
            initScale = transform.localScale;
        }

        public void OpenArea()
        {
            if (isOpened) return;
            isOpened = true;

            transform.DOScale(transform.localScale * scaleMultiplier, animationDuration).SetEase(Ease.OutBack);
            spriteRenderer.DOColor(openingColor, animationDuration).SetEase(Ease.Linear);
        }
        public void OpenArea(float delay = 0)
        {
            if (isOpened) return;
            isOpened = true;

            transform.DOScale(transform.localScale * scaleMultiplier, animationDuration).SetEase(Ease.OutBack).SetDelay(delay);
            spriteRenderer.DOColor(openingColor, animationDuration).SetEase(Ease.Linear).SetDelay(delay);
        }


        public void CloseArea()
        {
            if (!isOpened) return;
            isOpened = false;

            transform.DOScale(initScale, animationDuration).SetEase(Ease.OutBack);
            spriteRenderer.DOColor(Color.white, animationDuration).SetEase(Ease.Linear);
        }
    }
}