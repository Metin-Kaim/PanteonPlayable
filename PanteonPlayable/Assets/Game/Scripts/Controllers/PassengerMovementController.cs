using Assets.Game.Scripts.Signals;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.ShaderData;

namespace Assets.Game.Scripts.Controllers
{
    public class PassengerMovementController : MonoBehaviour
    {
        public List<PhasePoint> phasePoints;
        public PassengerManager passengerManager;
        public Transform baggage;
        public PassengerAnimationController animationController;

        int phaseNumber = -1;

        public void Move()
        {
            phaseNumber++;
            StartCoroutine(IMove(phasePoints[phaseNumber]));
        }

        public IEnumerator IMove(PhasePoint phasePoint)
        {
            int phasePointIndex = 0;

            List<Vector3> positions = phasePoint.points.Select(p => p.position).ToList();

            if (phasePoint.adaptive)
            {
                Vector3 newPos = passengerManager.PassengerOffsetWithoutBaggage(this);
                positions[positions.Count - 1] += newPos;
            }
            if (phasePoint.isRunning)
            {
                animationController.SetRun();
            }
            else
            {
                animationController.SetIdle();
            }

            phasePoint.itinialAction?.Invoke(this);

            while (phasePointIndex < positions.Count)
            {
                Vector3 direction = (positions[phasePointIndex] - transform.position).normalized;

                transform.position += phasePoint.speed * Time.deltaTime * direction;

                if (Mathf.Abs(Vector3.Distance(transform.position, positions[phasePointIndex])) < 0.05f)
                {
                    phasePointIndex++;
                }
                if (phasePoint.lookRotation)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
                }

                yield return null;
            }


            if (phasePoints[phaseNumber].join)
            {
                Move();
            }
            else
            {
                animationController.SetIdle();
            }

            if (phaseNumber == phasePoints.Count - 1)
            {
                gameObject.SetActive(false);
            }
        }

        public void GiveBaggageToPlayer()
        {
            if (baggage == null) return;

            Transform tempBaggage = baggage;
            baggage = null;

            Vector3 baggagePoint = PlayerSignals.Instance.onGetPlayerBaggagePoint.Invoke();
            PlayerSignals.Instance.onAddBaggage.Invoke(tempBaggage);

            tempBaggage.transform.DOLocalJump(baggagePoint, 3, 1, .3f).SetEase(Ease.Linear);
            tempBaggage.transform.DOLocalRotate(Vector3.forward * 90, .3f);

            Invoke(nameof(Move), .1f);
        }
    }
}