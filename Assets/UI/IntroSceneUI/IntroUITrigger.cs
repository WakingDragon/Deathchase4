using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class IntroUITrigger : MonoBehaviour
    {
        [SerializeField] private BoolGameEvent m_showIntroEvent = null;
        private bool startIntroCanvasAnim = false;

        private void Awake()
        {
            if (!m_showIntroEvent) { Debug.Log("No event on intro trigger"); }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                startIntroCanvasAnim = !startIntroCanvasAnim;
                m_showIntroEvent.Raise(startIntroCanvasAnim);
            }
        }
    }
}

