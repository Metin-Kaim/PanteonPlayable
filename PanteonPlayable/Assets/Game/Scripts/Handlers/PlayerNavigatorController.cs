using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Handlers
{
    public class PlayerNavigatorController : MonoBehaviour
    {
        [SerializeField] private float openDistance;
        [SerializeField] private float speed;
        [SerializeField] private GameObject navigateArrow;

        [SerializeField] private GameObject[] targets;

        private byte _targetIndex;
        private Transform _target;

        public void SetNextTarget()
        {
            targets[_targetIndex].SetActive(false);
            _targetIndex++;
            _target = targets[_targetIndex].transform;
            _target.gameObject.SetActive(true);
        }

        public void ClearTarget()
        {
            _target = null;
            navigateArrow.SetActive(false);
        }

        private void Update()
        {
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.position) < openDistance)
                {
                    navigateArrow.SetActive(false);
                    return;
                }
                else
                    navigateArrow.SetActive(true);

                Vector3 direction = (_target.position - transform.position).normalized;
                Quaternion targetRot = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.deltaTime);
            }
        }
    }
}