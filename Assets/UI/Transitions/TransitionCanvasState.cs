using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    [RequireComponent(typeof(UITransitions))]
    public class TransitionCanvasState : MonoBehaviour
    {
        //NOTE: This is very game specific
        [SerializeField] private VoidGameEvent transitionToBlack = null;
        [SerializeField] private VoidGameEvent transitionToView = null;

        public void OnPauseChange(bool newIsPaused)
        {
            if(newIsPaused)
            {
                transitionToBlack.Raise();
            }
            else
            {
                transitionToView.Raise();
            }
        }

    }
}

