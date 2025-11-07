using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class BaggageCheckController : MonoBehaviour
    {
        [SerializeField] private Transform baggageStackPoint;
        [SerializeField] private Vector3 baggageStackOffset;
        [SerializeField] private Transform carBaggagePoint;
        [SerializeField] private Transform conveyorLineEndPoint;
        [SerializeField] private Transform platformPointToJump;
        [SerializeField] private Transform platform;

        private List<Transform> baggages;

        public void TriggerEnterToStackBaggages()
        {
            baggages = PlayerSignals.Instance.onGetAllBaggages.Invoke();
            baggages.Reverse();

            for (int i = 0; i < baggages.Count; i++)
            {
                Transform baggage = baggages[i];
                baggage.SetParent(baggageStackPoint);

                baggage.DOLocalJump(i * baggageStackOffset, 3, 1, .4f).SetEase(Ease.Linear).SetDelay(i * .1f);
                baggage.DOLocalRotate(new Vector3(0, 180, 90), 0.4f).SetEase(Ease.Linear).SetDelay(i * .1f);
            }
        }

        public void MoveBaggagesToCar()
        {
            for (int i = 0; i < baggages.Count; i++)
            {
                Sequence seq = DOTween.Sequence();
                seq.SetEase(Ease.Linear);
                seq.SetDelay(i * 1.2f);

                Transform baggage = baggages[i];

                int i1 = i;
                seq.AppendCallback(() => baggage.SetParent(platformPointToJump));
                seq.Append(baggage.DOMove(conveyorLineEndPoint.position, 0.3f));
                seq.Join(baggageStackPoint.DOLocalMoveY(-.3f, 0.3f).SetRelative(true).SetEase(Ease.OutBounce).SetDelay(0.1f));
                seq.Append(baggage.DOLocalJump(Vector3.zero, 2, 1, .3f));
                seq.Join(baggage.DOLocalRotate(new Vector3(0, 90, 90), 0.3f));
                seq.Append(platform.DOLocalMoveY(4.5f, 0.2f).SetRelative(true).SetEase(Ease.OutSine));
                seq.AppendCallback(() => baggage.SetParent(carBaggagePoint));
                seq.Append(baggage.DOLocalJump(i1 * baggageStackOffset, 3, 1, .4f));
                seq.Join(platform.DOLocalMoveY(-4.5f, 0.2f).SetRelative(true).SetEase(Ease.InSine).SetDelay(0.05f));

                if (i == baggages.Count - 1)
                {
                    seq.AppendInterval(1);
                    seq.AppendCallback(() =>
                    {
                        carBaggagePoint.GetComponentInParent<CarController>().Move();
                    });
                }
            }
        }
    }
}