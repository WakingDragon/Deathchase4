using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
    {
    public class MenuSceneController : MonoBehaviour
    {
        [SerializeField] private BoolGameEvent m_toggleMainMenu = null;
        [SerializeField] private FloatVariable m_transitionDuration = null;

        [Header("music")]
        [SerializeField] private AudioCue m_ambientPadsCue = null;
        [SerializeField] private AudioCue m_menuRhythmCue = null;
        [SerializeField] private AudioCue m_menuCricketsCue = null;

        public void Activate()
        {
            if (m_ambientPadsCue) { StartCoroutine(AudioAmbience()); }
            if (m_menuRhythmCue) { m_menuRhythmCue.Play(); }
            if (m_menuCricketsCue) { m_menuCricketsCue.Play(); }

            StartCoroutine(ShowMenu());
        }

        private IEnumerator ShowMenu()
        {
            yield return new WaitForSeconds(m_transitionDuration.Value * 1.5f);
            m_toggleMainMenu.Raise(true);
        }

        private IEnumerator AudioAmbience()
        {
            while(true)
            {
                m_ambientPadsCue.Play();
                var t = Random.Range(10f, 20f);
                yield return new WaitForSecondsRealtime(t);
            }
        }

        public void Deactivate()
        {
            m_toggleMainMenu.Raise(false);
            //stop music?
        }
    }
}

