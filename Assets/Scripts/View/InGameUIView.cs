using UnityEngine;

namespace PokeyBallTest.View
{
    public class InGameUIView : MonoBehaviour
    {
        [SerializeField] private GameObject m_loseContainer = default;

        public void ShowLoseScreen()
        {
            m_loseContainer?.SetActive(true);
        }
    }
}

