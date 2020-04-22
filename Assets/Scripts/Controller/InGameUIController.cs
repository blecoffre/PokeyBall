using PokeyBallTest.Consts;
using PokeyBallTest.Manager;
using PokeyBallTest.View;
using UnityEngine;
using UnityEngine.Events;

namespace PokeyBallTest.Controller
{
    public class InGameUIController : MonoBehaviour
    {
        [SerializeField] private InGameUIView m_view = default;

        private UnityAction m_playerDieAction = null;

        private void Start()
        {
            SetActions();
        }

        private void SetActions()
        {
            m_playerDieAction += ProcessPlayerDie;
            EventManager.StartListening(EventsName.PlayerDie, m_playerDieAction);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventsName.PlayerDie, m_playerDieAction);
        }

        public void RetryCurrentLevel()
        {
            LevelManager.ReloadLevel();
        }

        private void ProcessPlayerDie()
        {
            m_view.ShowLoseScreen();
        }
    }

}
