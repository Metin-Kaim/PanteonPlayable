using UnityEngine;

namespace Assets.Game.Scripts.Handlers
{
    public class CameraPointHandler : MonoBehaviour
    {
        [SerializeField] private float camOrtographicSize;

        public float CamOrtographicSize => camOrtographicSize;
    }
}