using Assets.Game.Scripts.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private float playerWaitingDuration = 1;

        [Header("RunTime Variables")]
        [SerializeField] private List<GameObject> stairSteps;
        [SerializeField] private GameObject lastUsedStep;

        private GameObject _stepOfPlayer;
        private Coroutine _stairCoroutine;

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
                    step.transform.localPosition += moveSpeed * Time.deltaTime * (endPoint.localPosition - startPoint.localPosition).normalized;

                    float totalDistance = Vector3.Distance(startPoint.localPosition, endPoint.localPosition);
                    float currentDistance = Vector3.Distance(startPoint.localPosition, step.transform.localPosition);
                    if (currentDistance > totalDistance)
                    {
                        if (_stepOfPlayer == step)
                        {
                            ReleaseThePlayer();
                            _stepOfPlayer = null;
                        }

                        step.transform.localPosition = startPoint.localPosition;
                        lastUsedStep = step;

                    }
                }
                yield return null;
            }
        }
        private IEnumerator MoveThePlayerToEndPoint()
        {
            yield return new WaitForSeconds(playerWaitingDuration);

            _stairCoroutine = null;

            _stepOfPlayer = lastUsedStep;

            InputSignals.Instance.onDeactivateInput.Invoke();
            PlayerSignals.Instance.onClosePlayerCollider.Invoke();
            PlayerSignals.Instance.onSetPlayerParent.Invoke(lastUsedStep.transform);
        }

        public void ReleaseThePlayer()
        {
            PlayerSignals.Instance.onResetPlayerParent.Invoke();
            PlayerSignals.Instance.onOpenPlayerCollider.Invoke();
            InputSignals.Instance.onActivateInput.Invoke();

        }

        public void TriggerEnter()
        {
            if (_stairCoroutine != null) return;

            _stairCoroutine = StartCoroutine(MoveThePlayerToEndPoint());
        }

        public void TriggerExit()
        {
            if (_stairCoroutine != null)
            {
                StopCoroutine(_stairCoroutine);
                _stairCoroutine = null;
            }
        }

        //public void MovePassengerToUp(Transform target)
        //{
        //    StartCoroutine(IMovePassengerToUp(target));
        //}

        //public IEnumerator IMovePassengerToUp(Transform target)
        //{
        //    while (true)
        //    {
        //        target.transform.position += moveSpeed * Time.deltaTime * (endPoint.position - startPoint.position).normalized;

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