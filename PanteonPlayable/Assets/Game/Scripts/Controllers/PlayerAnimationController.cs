using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private Tweener _carryTweener;

        private void OnEnable()
        {
            PlayerSignals.Instance.onSetIdleAnimation += SetIdle;
            PlayerSignals.Instance.onSetRunAnimation += SetRun;
        }
        private void OnDisable()
        {
            PlayerSignals.Instance.onSetIdleAnimation -= SetIdle;
            PlayerSignals.Instance.onSetRunAnimation -= SetRun;
        }

        public void SetRun()
        {
            animator.SetTrigger("Run");
        }
        public void SetIdle()
        {
            animator.SetTrigger("Idle");
        }
        public void SetCarry(bool isCarrying)
        {
            _carryTweener?.Kill();
            _carryTweener = DOTween.To(() => animator.GetFloat("BlendCarry"), value => animator.SetFloat("BlendCarry", value), isCarrying ? 1 : 0, .5f);
        }
    }
}
