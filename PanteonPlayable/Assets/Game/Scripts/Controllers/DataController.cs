using Assets.Game.Scripts.Datas;
using Assets.Game.Scripts.Signals;
using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class DataController : MonoBehaviour
    {
        [SerializeField] private ColorSetSO colorSetSO;

        private void OnEnable()
        {
            DataSignals.Instance.onGetRandomColorMaterial += colorSetSO.GetRandomColorMaterial;
        }
        private void OnDisable()
        {
            DataSignals.Instance.onGetRandomColorMaterial -= colorSetSO.GetRandomColorMaterial;
        }
    }
}