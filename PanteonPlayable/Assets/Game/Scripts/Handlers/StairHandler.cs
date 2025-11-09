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
        [SerializeField] private bool isDummy;

        [Header("RunTime Variables")]
        [SerializeField] private List<GameObject> stairSteps;

        private List<Vector3> initialStepPositions = new List<Vector3>();

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

            foreach (var step in stairSteps)
            {
                initialStepPositions.Add(step.transform.localPosition);
            }

            StartCoroutine(MoveSteps());
        }

        private IEnumerator MoveSteps()
        {
            byte moveStep = 0;

            while (true)
            {
                foreach (var step in stairSteps)
                {
                    Vector3 direction = (endPoint.localPosition - step.transform.localPosition).normalized;
                    step.transform.localPosition += moveSpeed * Time.deltaTime * direction;

                    if (Mathf.Abs(Vector3.Distance(step.transform.localPosition, endPoint.localPosition)) < 0.02f)
                    {
                        step.transform.localPosition = startPoint.localPosition;
                        moveStep++;
                    }
                }

                if (moveStep == stairSteps.Count)
                {
                    for (int i = 0; i < moveStep; i++)
                    {
                        stairSteps[i].transform.localPosition = initialStepPositions[i];
                    }
                    moveStep = 0;
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

                Vector3 direction = (selectedPos - player.position).normalized;
                player.position += moveSpeed * Time.deltaTime * direction;

                if (direction != Vector3.zero)
                {
                    Vector3 flatDir = new Vector3(direction.x, 0, direction.z); // sadece yatay eksende
                    Quaternion targetRot = Quaternion.LookRotation(flatDir);
                    player.rotation = Quaternion.Slerp(
                        player.rotation,
                        targetRot,
                        Time.deltaTime * 10f // dönüş hızı
                    );
                }

                if (Mathf.Abs(Vector3.Distance(player.position, selectedPos)) < 0.05f)
                {
                    pathIndex++;
                }


                yield return null;
            }
            ReleaseThePlayer();
            triggerController.gameObject.SetActive(false);
            if (!isDummy)
            {
                isDummy = true;
                PlayerSignals.Instance.onSetNextTarget.Invoke();
            }
            else
            {
                PlayerSignals.Instance.onOpenNavigation.Invoke();
            }
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
    }
}