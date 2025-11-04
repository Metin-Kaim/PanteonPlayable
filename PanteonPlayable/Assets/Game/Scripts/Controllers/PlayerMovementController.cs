using Assets.Game.Scripts.Signals;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector2 joystickDirection = InputSignals.Instance.onGetInput.Invoke();

            if (joystickDirection != Vector2.zero)
            {
                Vector3 inputDir = new(joystickDirection.y, 0, -joystickDirection.x);

                Quaternion targetRotation = Quaternion.LookRotation(inputDir);

                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

                Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
                _rb.MovePosition(_rb.position + move);
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
        }
    }
}