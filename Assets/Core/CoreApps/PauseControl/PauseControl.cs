using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class PauseControl : MonoBehaviour
    {
        //pause key and pause action are slightly different. Pause action could be triggered by menus for example
        [SerializeField] private BoolVariable m_isPaused = null;
        [SerializeField] private BoolGameEvent m_notifyPauseKey = null;
        [SerializeField] private BoolGameEvent m_notifyPauseAction = null;
        private bool m_pauseKeyActive = true;

        private void Awake()
        {
            if(!m_isPaused || !m_notifyPauseKey || !m_notifyPauseAction)
            { Debug.Log("no pause events/vars"); }
        }

        public void OnPauseKeyPressed()
        {
            if(m_pauseKeyActive)
            {
                m_isPaused.Value = !m_isPaused.Value;
                m_notifyPauseKey.Raise(m_isPaused.Value);
                m_notifyPauseAction.Raise(m_isPaused.Value);

                //Debug.Log("pause key:" + m_isPaused.Value);
            }
        }

        public void OnExternalPauseRequest(bool isPaused)
        {
            m_pauseKeyActive = !isPaused;
            if(isPaused != m_isPaused.Value)
            {
                m_notifyPauseAction.Raise(m_isPaused.Value);
            }
            m_isPaused.Value = isPaused;
            //Debug.Log("request pause:" + isPaused);
        }
    }
}

