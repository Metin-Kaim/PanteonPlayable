using Assets.Game.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] private float moveToTargetDuration = 1;

        private Vector3 _followOffset;
        private bool _canFollow;
        private Vector3 lastPosition;
        private Vector3 lastRotationAngles;

        private void OnEnable()
        {
            CameraSignals.Instance.onMoveToTarget += MoveToTarget;
            CameraSignals.Instance.onBackToBase += BackToBase;
        }
        private void OnDisable()
        {
            CameraSignals.Instance.onMoveToTarget -= MoveToTarget;
            CameraSignals.Instance.onBackToBase -= BackToBase;
        }

        private void Start()
        {
            _followOffset = transform.position - PlayerSignals.Instance.onGetPlayerPosition.Invoke();
        }

        private void LateUpdate()
        {
            if (_canFollow)
                transform.position = PlayerSignals.Instance.onGetPlayerPosition.Invoke() + _followOffset;
        }

        private void MoveToTarget()
        {
            _canFollow = false;

            lastPosition = transform.position;
            lastRotationAngles = transform.rotation.eulerAngles;

            transform.DOMove(target.position, moveToTargetDuration).SetEase(Ease.Linear);
            transform.DORotate(target.rotation.eulerAngles, moveToTargetDuration).SetEase(Ease.Linear);
        }
        private void BackToBase()
        {
            transform.DOMove(lastPosition, moveToTargetDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                InputSignals.Instance.onActivateInput.Invoke();
            });
            transform.DORotate(lastRotationAngles, moveToTargetDuration).SetEase(Ease.Linear);
        }
    }
}