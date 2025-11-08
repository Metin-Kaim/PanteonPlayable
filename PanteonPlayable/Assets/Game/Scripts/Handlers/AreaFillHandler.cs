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

        private void Start()
        {
            initScale = transform.localScale;
        }

        public void OpenArea()
        {
            transform.DOScale(transform.localScale * scaleMultiplier, animationDuration).SetEase(Ease.OutBack);
            spriteRenderer.DOColor(openingColor, animationDuration).SetEase(Ease.Linear);
        }

        public void CloseArea()
        {
            transform.DOScale(initScale, animationDuration).SetEase(Ease.OutBack);
            spriteRenderer.DOColor(Color.white, animationDuration).SetEase(Ease.Linear);
        }
    }
}