using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Controllers
{
    public class TriggerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Transform> onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(transform);
        }

        private void OnDisable()
        {
            onTriggerExit.Invoke();
        }
    }
}