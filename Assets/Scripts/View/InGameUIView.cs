using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIView : MonoBehaviour
{
    [SerializeField] private GameObject m_loseContainer = default;

    public void ShowLoseScreen()
    {
        m_loseContainer?.SetActive(true);
    }
}
