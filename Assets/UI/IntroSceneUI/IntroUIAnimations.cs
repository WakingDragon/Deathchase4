using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.UI
{
    public class IntroUIAnimations : MonoBehaviour
    {
        [SerializeField] private float m_enterIntroTime = 1f;
        [SerializeField] private AudioCue m_enterIntroSFX = null;
        [SerializeField] private float m_exitIntroTime = 1f;
        [SerializeField] private AudioCue m_exitIntroSFX = null;

        //[Header("dependencies")]
        [SerializeField] private CanvasGroup canvasGroup;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            canvasGroup.gameObject.SetActive(false);
        }

        public void OnNotifyShowIntroCanvas(bool enterCanvas)
        {
            if(enterCanvas)
            {
                StartCoroutine(EnterIntro());
            }
            else
            {
                StartCoroutine(ExitIntro());   
            }
        }

        private IEnumerator EnterIntro()
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            animator.SetTrigger(UIStatics.enterCanvasAnimTrigger);
            if (m_enterIntroSFX) { m_enterIntroSFX.Play(); }
            yield return new WaitForSecondsRealtime(m_enterIntroTime);
        }

        private IEnumerator ExitIntro()
        {
            animator.SetTrigger(UIStatics.exitCanvasAnimTrigger);
            if (m_exitIntroSFX) { m_exitIntroSFX.Play(); }
            yield return new WaitForSecondsRealtime(m_exitIntroTime);
            canvasGroup.gameObject.SetActive(false);
        }
    }
}


