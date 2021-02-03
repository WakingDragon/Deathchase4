using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class CountdownTrigger : MonoBehaviour
    {
        //optional class
        [SerializeField] private VoidGameEvent m_startCountdownEvent = null;

        private void Awake()
        {
            if (!m_startCountdownEvent) { Debug.Log("No start countdown event on trigger"); }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                m_startCountdownEvent.Raise();
            }
        }
    }
}

