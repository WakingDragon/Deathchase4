using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class PauseUIAnimations : MonoBehaviour
    {
        [SerializeField] private AudioCue enterPauseSFX = null;
        [SerializeField] private AudioCue exitPauseSFX = null;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnGamePauseChange(bool newIsPaused)
        {
            if(newIsPaused)
            {
                animator.SetTrigger(UIStatics.enterCanvasAnimTrigger);
                if(enterPauseSFX) { enterPauseSFX.Play(); }
            }
            else
            {
                animator.SetTrigger(UIStatics.exitCanvasAnimTrigger);
                if (exitPauseSFX) { exitPauseSFX.Play(); }
            }
        }
    }
}


