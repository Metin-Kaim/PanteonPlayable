using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Game.Scripts.Controllers
{
    public class TriggerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke();
        }
    }
}