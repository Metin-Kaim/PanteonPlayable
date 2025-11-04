using Assets.Game.Scripts.Signals;
using UnityEngine;

public class DataController : MonoBehaviour
{
    [SerializeField] Joystick joystick;

    private void OnEnable()
    {
        DataSignals.Instance.onGetJoystickDirection += OnGetJoystickDirection;
    }

    private Vector2 OnGetJoystickDirection()
    {
        return joystick.Direction;
    }

    private void OnDisable()
    {
        DataSignals.Instance.onGetJoystickDirection -= OnGetJoystickDirection;
    }
}
