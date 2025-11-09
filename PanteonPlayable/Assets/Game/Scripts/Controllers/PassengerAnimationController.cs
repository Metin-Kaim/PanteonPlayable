using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class PassengerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private bool _isRunning;

        private void Start()
        {
            SetCarry(true);
        }

        public void SetRun()
        {
            if (_isRunning) return;
            _isRunning = true;

            animator.SetTrigger("Run");
        }
        public void SetIdle()
        {
            if (!_isRunning) return;
            _isRunning = false;

            animator.SetTrigger("Idle");
        }
        public void SetCarry(bool isCarrying)
        {
            DOTween.To(() => animator.GetFloat("BlendCarry"), value => animator.SetFloat("BlendCarry", value), isCarrying ? 1 : 0, .5f);
        }
    }
}