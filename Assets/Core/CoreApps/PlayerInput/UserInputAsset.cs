using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    [CreateAssetMenu(fileName ="userInputLibrary",menuName ="Core/Input/(single) User Input Library")]
    public class UserInputAsset : ScriptableObject
    {
        [Header("anykey")]
        [SerializeField] private VoidGameEvent m_anyKeyEvent = null;

        [Header("axes")]
        [SerializeField] private float m_axisSensitivity = 0.01f;
        [SerializeField] private FloatGameEvent m_xAxisEvent = null;
        [SerializeField] private FloatGameEvent m_yAxisEvent = null;
        private float m_lastX, m_lastY;

        [Header("fire and jump")]
        [SerializeField] private BoolGameEvent m_fire = null;
        [SerializeField] private VoidGameEvent m_fireBtnDownOnly = null;

        [Header("keyboard keys")]
        [SerializeField] private VoidGameEvent m_PkeyDown = null;
        [SerializeField] private VoidGameEvent m_ESCkeyDown = null;

        public void CheckForInputs()
        {
            AnyKeyDown();
            XAxisInput();
            YAxisInput();
            Fire1Input();
            OtherKeysDown();
        }

        private void AnyKeyDown()
        {
            if(Input.anyKeyDown)
            {
                m_anyKeyEvent.Raise();
            }
        }

        private void XAxisInput()
        {
            var xAxis = Input.GetAxis("Horizontal");

            if (Mathf.Abs(xAxis) > m_axisSensitivity)
            {
                m_xAxisEvent.Raise(xAxis);
                m_lastX = xAxis;
            }
            else if (Mathf.Abs(m_lastX) > m_axisSensitivity)
            {
                m_xAxisEvent.Raise(0f);
                m_lastX = 0f;
            }
        }

        private void YAxisInput()
        {
            var yAxis = Input.GetAxis("Vertical");

            if (Mathf.Abs(yAxis) > m_axisSensitivity)
            {
                m_yAxisEvent.Raise(yAxis);
                m_lastY = yAxis;
            }
            else if (Mathf.Abs(m_lastY) > m_axisSensitivity)
            {
                m_yAxisEvent.Raise(0f);
                m_lastY = 0f;
            }
        }

        private void Fire1Input()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                m_fire.Raise(true);
                m_fireBtnDownOnly.Raise();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                m_fire.Raise(false);
            }
        }

        private void OtherKeysDown()
        {
            if(Input.GetKeyDown(KeyCode.Escape)) { m_ESCkeyDown.Raise(); }
            if (Input.GetKeyDown(KeyCode.P)) { m_PkeyDown.Raise(); }
        }
    }
}

