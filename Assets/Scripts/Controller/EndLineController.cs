using PokeyBallTest.Consts;
using UnityEngine;

namespace PokeyBallTest.Controller
{
    public class EndLineController : MonoBehaviour
    {
        [SerializeField] private GameObject m_nextLevelPortal = default;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagsName.Player))
            {
                Invoke("OpenPortal", 0.5f); //Call "OpenPortal" after a small delay, used to avoid player trigger the portal
            }
        }

        private void OpenPortal()
        {
            m_nextLevelPortal?.SetActive(true);
        }
    }
}

