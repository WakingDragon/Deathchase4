using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

public class SunRotator : MonoBehaviour
{
    [SerializeField] private float m_baseSpeed = 20f;
    private float m_speed;
    [SerializeField] private float m_startAngle = 270f;
    [SerializeField] private float m_worldOrientation = 90f;
    private float m_angle;
    [SerializeField] private BoolGameEvent m_itGotDarkEvent = null;
    [SerializeField] private BoolVariable m_isDark = null;
    private bool m_previousDark;

    private void Awake()
    {
        m_angle = m_startAngle;
        m_speed = m_baseSpeed;
        m_isDark.Value = true;  //set to opposite to trigger a notification
    }

    private void LateUpdate()
    {
        RecalcXAngle();
        transform.rotation = Quaternion.Euler(new Vector3(m_angle, m_worldOrientation, 0f));
        CheckIfItsDark(m_angle);
    }

    private void RecalcXAngle()
    {
        m_angle = Utils.ModF(m_angle,360f);

        if(m_angle > 300f || m_angle < 60f)
        {
            m_speed = m_baseSpeed;
        }
        else if(m_angle > 275f || m_angle < 85f)
        {
            m_speed = m_baseSpeed / 2f;
        }
        else
        {
            m_speed = m_baseSpeed * 2f;
        }

        m_angle = m_angle + (m_speed * Time.deltaTime);
    }

    private void CheckIfItsDark(float angle)
    {
        if(angle < 280f && angle > 70f)
        { m_isDark.Value = true; }
        else
        { m_isDark.Value = false; }

        if (m_isDark.Value != m_previousDark)
        {
            m_itGotDarkEvent.Raise(m_isDark.Value);
            m_previousDark = m_isDark.Value;
        }
    }
}
