using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLights : MonoBehaviour
{
    [SerializeField] private GameObject m_light = null;
    [SerializeField] private BoolVariable m_isDark = null;

    private void Start()
    {
        m_light.SetActive(m_isDark.Value);
    }

    public void OnNewDark(bool newDark)
    {
        m_light.SetActive(newDark);
    }
}
