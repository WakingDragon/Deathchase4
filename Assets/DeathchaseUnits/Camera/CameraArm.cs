using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Deathchase
{
    public class CameraArm : MonoBehaviour
    {
        [SerializeField] private Vector3Var m_closeOffset = null;
        [SerializeField] private Vector3Var m_widePanOffset = null;
        private Vector3 m_offset;

        private void Awake()
        {
            if(!m_closeOffset) { Debug.Log("no offset var in cam arm"); }
            if (!m_widePanOffset) { Debug.Log("no offset var in cam arm"); }
            m_offset = m_widePanOffset.Value;
            SetNewOffset(m_offset);
        }

        public void SetNewOffset(Vector3 offset)
        {
            transform.localPosition = offset;
        }

        public void ToggleWidePanOffset(bool isWidePan)
        {
            if(isWidePan)
            {
                m_offset = m_widePanOffset.Value;
            }
            else
            {
                m_offset = m_closeOffset.Value;
            }
            SetNewOffset(m_offset);
        }
    }
}

