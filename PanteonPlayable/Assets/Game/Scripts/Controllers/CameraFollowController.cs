using Assets.Game.Scripts.Handlers;
using Assets.Game.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] private float moveToTargetDuration = 1;
        [SerializeField] private Camera cam;

        private Vector3 _followOffset;
        private bool _canFollow = true;
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
        public void MoveToTarget(CameraPointHandler camInfos)
        {
            LockCamFollow();

            lastPosition = transform.position;
            lastRotationAngles = transform.rotation.eulerAngles;

            transform.DOMove(camInfos.transform.position, moveToTargetDuration).SetEase(Ease.Linear);
            transform.DORotate(camInfos.transform.rotation.eulerAngles, moveToTargetDuration).SetEase(Ease.Linear);
            cam.DOOrthoSize(camInfos.CamOrtographicSize, moveToTargetDuration).SetEase(Ease.Linear);
        }
        private void BackToBase()
        {
            transform.DOMove(lastPosition, moveToTargetDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                InputSignals.Instance.onActivateInput.Invoke();
                UnlockCamFollow();
            });
            transform.DORotate(lastRotationAngles, moveToTargetDuration).SetEase(Ease.Linear);
        }

        public void LockCamFollow()
        {
            _canFollow = false;
        }
        public void UnlockCamFollow()
        {
            _canFollow = true;
        }
    }
}