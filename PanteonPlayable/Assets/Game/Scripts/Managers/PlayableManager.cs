using Assets.Game.Scripts.Signals;
using Luna.Unity;
using UnityEngine;

namespace Assets.Game.Scripts.Managers
{
    public class PlayableManager : MonoBehaviour
    {
        private bool _isGameEnded;

        private void OnEnable()
        {
            PlayableSignals.Instance.onGoToStore += GoToStore;
        }
        private void OnDisable()
        {
            PlayableSignals.Instance.onGoToStore -= GoToStore;
        }

        public void GoToStore()
        {
            if (_isGameEnded) return;

            _isGameEnded = true;

            InputSignals.Instance.onDeactivateInput.Invoke();
            LifeCycle.GameEnded();
            //Playable.InstallFullGame();
            print("Store Opened");
        }
    }
}