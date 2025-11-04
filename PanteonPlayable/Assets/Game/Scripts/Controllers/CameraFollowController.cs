using Assets.Game.Scripts.Signals;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class CameraFollowController : MonoBehaviour
    {
        [SerializeField] Vector3 followOffset;

        private void Start()
        {
            followOffset = transform.position - PlayerSignals.Instance.onGetPlayerPosition.Invoke();
        }

        private void LateUpdate()
        {
            transform.position = PlayerSignals.Instance.onGetPlayerPosition.Invoke() + followOffset;
        }
    }
}