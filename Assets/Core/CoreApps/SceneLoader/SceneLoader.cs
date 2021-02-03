using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BP.Core
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("dependencies")]
        [SerializeField] private ScenesLibrary m_sceneLibrary = null;
        [SerializeField] private BoolGameEvent m_pausedForLoadingEvent = null;

        private void Awake()
        {
            if(!m_sceneLibrary) { Debug.Log("scene loader missing assets"); }
            m_sceneLibrary.Activate();
        }

        #region loading/unloading
        public IEnumerator LoadAdditive(SceneAsset scene, bool setActiveOnLoad)
        {
            m_sceneLibrary.AddSceneToLibrary(scene);

            m_pausedForLoadingEvent.Raise(true);

            AsyncOperation asyncOp;

            asyncOp = SceneManager.LoadSceneAsync(scene.SceneName(), LoadSceneMode.Additive);

            asyncOp.allowSceneActivation = false;

            yield return MonitorLoadingOfScene(asyncOp);    //change status @90%

            asyncOp.allowSceneActivation = setActiveOnLoad;

            yield return CheckIfLoadIsDone(asyncOp);

            //update the scene asset
            m_sceneLibrary.SetAsLoaded(scene);
            var sceneRef = SceneManager.GetSceneByName(scene.SceneName());
            scene.SetSceneRef(sceneRef);
            if(setActiveOnLoad)
            {
                SceneManager.SetActiveScene(sceneRef);
                m_sceneLibrary.SetActiveScene(scene);
            }
            scene.OnLoad();

            m_pausedForLoadingEvent.Raise(false);
        }

        private IEnumerator MonitorLoadingOfScene(AsyncOperation asyncScene)
        {
            while (asyncScene.progress < 0.9f)
            {
                yield return null;
            }
        }

        private IEnumerator CheckIfLoadIsDone(AsyncOperation asyncScene)
        {
            while (!asyncScene.isDone)
            {
                yield return null;
            }
        }

        public void UnloadScene(SceneAsset scene)
        {
            scene.OnUnload();
            SceneManager.UnloadSceneAsync(scene.SceneName());
            scene.SetActive(false);
            m_sceneLibrary.SetAsUnloaded(scene);
        }
        #endregion 
    }
}

