using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class ClickButtonRaiseVoidEvent : MonoBehaviour
    {
        [SerializeField] private VoidGameEvent m_eventToRaise = null;
        [SerializeField] private AudioCue m_buttonClickAudio = null;

        public void ButtonClicked()
        {
            m_eventToRaise.Raise();
            if(m_buttonClickAudio) { m_buttonClickAudio.Play(); }
        }
    }
}

