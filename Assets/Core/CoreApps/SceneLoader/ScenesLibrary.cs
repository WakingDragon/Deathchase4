using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BP.Core
{
    [CreateAssetMenu(fileName = "SceneLibrary", menuName = "Core/Scenes/(Single) Scene Library Asset")]
    public class ScenesLibrary : ScriptableObject
    {        
        [Header("dependencies")]
        [SerializeField] private SceneAssetGameEvent onSceneLoadedEvent = null;
        [SerializeField] private SceneAssetGameEvent onSceneUnloadedEvent = null;
        
        [Header("Master Scenes Library")]
        [SerializeField] private SceneAsset m_coreScene = null;
        private List<SceneAsset> m_sceneLibrary = new List<SceneAsset>();
        [SerializeField] private List<SceneAsset> m_loadedScenes = new List<SceneAsset>();

        #region compilation
        public void Activate()
        {
            AddSceneToLibrary(m_coreScene);
            SetAsLoaded(m_coreScene);
        }

       public void AddSceneToLibrary(SceneAsset sceneAsset)
        {
            if (!m_sceneLibrary.Contains(sceneAsset)) { m_sceneLibrary.Add(sceneAsset); }
        }
        #endregion

        #region scene status
        public void SetAsLoaded(SceneAsset scene)
        {
            scene.SetLoaded(true);
            if (!m_loadedScenes.Contains(scene)) { m_loadedScenes.Add(scene); }
        }
        public void SetAsUnloaded(SceneAsset scene)
        {
            scene.SetLoaded(false);
            if (m_loadedScenes.Contains(scene)) { m_loadedScenes.Remove(scene); }
        }

        public void SetActiveScene(SceneAsset activeScene)
        {
            foreach(SceneAsset scene in m_sceneLibrary)
            {
                if(scene == activeScene)
                {
                    scene.SetActive(true);
                }
                else
                {
                    scene.SetActive(false);
                }
            }
        }

        public string GetActiveScene()
        {
            foreach (SceneAsset scene in m_sceneLibrary)
            {
                if(scene.IsActive())
                {
                    return scene.SceneName();
                }
            }
            return null;
        }
        #endregion

    }
}


