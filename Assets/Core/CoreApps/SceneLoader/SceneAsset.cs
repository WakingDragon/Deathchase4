using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BP.Core
{
    [CreateAssetMenu(fileName ="new_sceneAsset",menuName ="Core/Scenes/New Scene Asset")]
    public class SceneAsset : ScriptableObject
    {
        [SerializeField] private string sceneName = "MUST be name of scene in build settings";

        [SerializeField] private VoidGameEvent[] onLoadEvents;
        [SerializeField] private VoidGameEvent[] onUnloadEvents;

        //private int sceneIndex;
        [SerializeField] private bool m_isLoaded;
        [SerializeField] private bool m_isActive;
        private Scene m_sceneRef;

        private void OnEnable()
        {
            m_isLoaded = false;
            m_isActive = false;
        }

        #region getters/setters
        public string SceneName() { return sceneName; }
        public bool IsLoaded() { return m_isLoaded; }
        public void SetLoaded(bool isLoaded) { m_isLoaded = isLoaded; }
        public bool IsActive() { return m_isActive; }
        public void SetActive(bool isActive) { m_isActive = isActive; }

        public Scene SceneRef() { return m_sceneRef; }
        public void SetSceneRef()
        { m_sceneRef = SceneManager.GetSceneByName(sceneName); }
        public void SetSceneRef(Scene sceneRef) { m_sceneRef = sceneRef; }
        #endregion

        public void OnLoad()
        {
            for(int i = 0; i < onLoadEvents.Length; i++)
            {
                onLoadEvents[i].Raise();
            }
        }

        public void OnUnload()
        {
            for (int i = 0; i < onUnloadEvents.Length; i++)
            {
                onUnloadEvents[i].Raise();
            }
        }
    }
}

