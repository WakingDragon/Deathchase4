using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
    {
    public class IntroSceneController : MonoBehaviour
    {
        
        [SerializeField] private AudioCue m_ambientPadsCue = null;
        [SerializeField] private BoolGameEvent m_showIntroUI = null;

        [SerializeField] private SceneAssetGameEvent m_triggerNextSceneEvent = null;
        //[SerializeField] private SceneAsset m_nextScene = null;
        [SerializeField] private VoidGameEvent m_triggerIntroCompleted = null;

        private void Awake()
        {
            if(!m_triggerNextSceneEvent)
            { Debug.Log("cannot load next scene cos no assets"); }
        }


        public void StartIntroSequence()
        {
            StartCoroutine(IntroSequence());
        }

        private IEnumerator IntroSequence()
        {
            m_ambientPadsCue.Play();
            yield return new WaitForSeconds(1f);
            m_showIntroUI.Raise(true);
            yield return new WaitForSeconds(5f);
            m_showIntroUI.Raise(false);
            yield return new WaitForSeconds(1f);
            //m_triggerNextSceneEvent.Raise(m_nextScene);
            m_triggerIntroCompleted.Raise();
        }
    }
}

