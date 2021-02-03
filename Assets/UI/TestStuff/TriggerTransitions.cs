using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class TriggerTransitions : MonoBehaviour
    {
        [SerializeField] private VoidGameEvent startTransitionEvent = null;
        [SerializeField] private VoidGameEvent endTransitionEvent = null;
        private bool isPaused = false;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if(!isPaused)
                {
                    startTransitionEvent.Raise();
                }
                else
                {
                    endTransitionEvent.Raise();
                }
                isPaused = !isPaused;
            }
        }
    }


}

