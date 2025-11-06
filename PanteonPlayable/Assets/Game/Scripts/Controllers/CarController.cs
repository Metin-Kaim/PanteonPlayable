using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Transform baggageStackPoint;
        [SerializeField] private Transform carArrivePoint;
        [SerializeField] private Transform baggagePlanePoint;

        private Vector3 initialPos;

        private void Start()
        {
            initialPos = transform.position;
        }

        public void Move()
        {
            transform.DOMove(carArrivePoint.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                int childCount = baggageStackPoint.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    int i1 = i;

                    Transform baggage = baggageStackPoint.GetChild(0);
                    baggage.SetParent(baggagePlanePoint);
                    baggage.DOLocalJump(Vector3.zero, 3, 1, .5f).SetDelay(i * 0.1f).OnComplete(() =>
                    {
                        baggage.gameObject.SetActive(false);
                        if (i1 == childCount - 1)
                        {
                            transform.DOMove(initialPos, .5f).SetEase(Ease.Linear);
                        }
                    });
                }
            });
        }
    }
}