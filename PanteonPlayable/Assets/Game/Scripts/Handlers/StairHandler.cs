using Assets.Game.Scripts.Controllers;
using Assets.Game.Scripts.Signals;
using DG.Tweening;
using Luna.Unity.FacebookInstantGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Game.Scripts.Handlers
{
    public class StairHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject stairStep;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private byte stepCount;
        [SerializeField] private float moveSpeed = 1;

        [Header("RunTime Variables")]
        [SerializeField] private List<GameObject> stairSteps;

        private void Start()
        {
            stairSteps = new List<GameObject>();

            Vector3 stepPos = startPoint.localPosition;

            float differenceX = startPoint.localPosition.x - endPoint.localPosition.x;
            float differenceY = startPoint.localPosition.y - endPoint.localPosition.y;

            float offsetX = differenceX / stepCount;
            float offsetY = differenceY / stepCount;

            for (int i = 0; i < stepCount + 1; i++)
            {
                GameObject step = Instantiate(stairStep, transform);
                stairSteps.Add(step);
                step.transform.localPosition = stepPos;
                stepPos -= new Vector3(offsetX, offsetY, 0);
                step.name = $"Step{i}";
            }

            GameObject additionalStep = Instantiate(stairStep, transform);
            additionalStep.transform.localPosition = stairSteps[stairSteps.Count - 1].transform.localPosition;
            stairSteps.Add(additionalStep);
            additionalStep.name = $"Step{stairSteps.Count - 1}";

            stairSteps.RemoveAt(stairSteps.Count - 2);
            stairSteps.RemoveAt(0);

            StartCoroutine(MoveSteps());
        }

        private IEnumerator MoveSteps()
        {
            while (true)
            {
                foreach (var step in stairSteps)
                {
                    Vector3 direction = (endPoint.localPosition - step.transform.localPosition).normalized;
                    step.transform.localPosition += moveSpeed * Time.deltaTime * direction;

                    if (Mathf.Abs(Vector3.Distance(step.transform.localPosition, endPoint.localPosition)) < 0.05f)
                    {
                        step.transform.localPosition = startPoint.localPosition;
                    }
                }
                yield return null;
            }
        }
        private IEnumerator MoveThePlayerToEndPoint(Transform triggerController)
        {
            InputSignals.Instance.onDeactivateInput.Invoke();
            PlayerSignals.Instance.onClosePlayerCollider.Invoke();

            Transform player = PlayerSignals.Instance.onGetPlayer.Invoke();

            Vector3[] stairPath = new Vector3[]
            {
                startPoint.position,
                endPoint.position,
                endPoint.position + endPoint.forward
            };

            byte pathIndex = 0;

            while (pathIndex < stairPath.Length)
            {
                Vector3 selectedPos = stairPath[pathIndex];

                Vector3 direction = (selectedPos - player.transform.position).normalized;
                player.transform.position += moveSpeed * Time.deltaTime * direction;

                if (direction != Vector3.zero)
                {
                    Vector3 flatDir = new Vector3(direction.x, 0, direction.z); // sadece yatay eksende
                    Quaternion targetRot = Quaternion.LookRotation(flatDir);
                    player.transform.rotation = Quaternion.Slerp(
                        player.transform.rotation,
                        targetRot,
                        Time.deltaTime * 10f // dönüş hızı
                    );
                }

                if (Mathf.Abs(Vector3.Distance(player.transform.position, selectedPos)) < 0.05f)
                {
                    pathIndex++;
                }


                yield return null;
            }
            ReleaseThePlayer();
            triggerController.gameObject.SetActive(false);
        }

        public void ReleaseThePlayer()
        {
            PlayerSignals.Instance.onOpenPlayerCollider.Invoke();
            InputSignals.Instance.onActivateInput.Invoke();
        }

        public void TriggerEnter(Transform triggerController)
        {
            StartCoroutine(MoveThePlayerToEndPoint(triggerController));
        }

        public void TriggerExit()
        { }

        //public void MovePassengerToUp(Transform target)
        //{
        //    StartCoroutine(IMovePassengerToUp(target));
        //}

        //public IEnumerator IMovePassengerToUp(Transform target)
        //{
        //    while (true)
        //    {
        //        target.transform.position += moveSpeed * Time.deltaTime * (endPoint.position - startPoint.position).direction;

        //        float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        //        float currentDistance = Vector3.Distance(startPoint.position, target.transform.position);
        //        if (currentDistance > totalDistance)
        //        {
        //            //target.GetComponent<PassengerMovementController>().Move();
        //            yield break;
        //        }
        //        yield return null;
        //    }
        //}
    }
}