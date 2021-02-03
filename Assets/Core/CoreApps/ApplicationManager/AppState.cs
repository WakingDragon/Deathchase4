using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class AppState : MonoBehaviour
    {
        /// <summary>
        /// need to stay focussed on what can be relegated from some uber manager to in-scene functions
        /// taking the approach of building this first to show the different things we will need before then splitting it out into some kind of SO/object-based method
        /// </summary>

        [SerializeField] private SceneAsset m_uiScene = null;
        [SerializeField] private SceneAsset m_introScene = null;
        [SerializeField] private SceneAsset m_menuScene = null;
        [SerializeField] private SceneAsset m_gameScene = null;

        [Header("dependencies")]
        [SerializeField] private BoolGameEvent m_toggleMainMenu = null;
        [SerializeField] private FloatVariable m_transitionDurationVar = null;
        [SerializeField] private VoidGameEvent m_transitionToBlack = null;
        [SerializeField] private VoidGameEvent m_transitionBackToView = null;
        private CameraManager camManager;
        private SceneLoader sceneLoader;

        private void Awake()
        {
            camManager = FindObjectOfType<CameraManager>();
            if (!camManager) { Debug.Log("no cam manager found"); }

            sceneLoader = FindObjectOfType<SceneLoader>();
            if (!sceneLoader) { Debug.Log("no sceneloader found"); }
        }

        private void Start()
        {
            StartCoroutine(ProgressToIntro());
        }

        private IEnumerator ProgressToIntro()
        {
            yield return StartCoroutine(LoadScene(m_uiScene, true));
            StartCoroutine(LoadScene(m_introScene, true));
            
        }

        public void LoadMenuEventResponse()
        {
            StartCoroutine(TransitionBetweenScenes(m_menuScene,m_introScene));
            //StartCoroutine(DebugEnumerator());
        }

        //private IEnumerator DebugEnumerator()
        //{
        //    yield return new WaitForSeconds(2f);
        //    MainMenuClickPlay();
        //}

        public void MainMenuClickPlay()
        {
            StartCoroutine(TransitionBetweenScenes(m_gameScene, m_menuScene));
        }

        #region generic functions TODO: consider moving to SceneLoader
        private IEnumerator LoadScene(SceneAsset scene, bool setActiveOnLoad)
        {
            yield return StartCoroutine(sceneLoader.LoadAdditive(scene, setActiveOnLoad));
        }

        private IEnumerator TransitionBetweenScenes(SceneAsset newScene, SceneAsset oldScene)
        {
            m_transitionToBlack.Raise();
            yield return new WaitForSeconds(m_transitionDurationVar.Value);

            yield return StartCoroutine(sceneLoader.LoadAdditive(newScene, true));
            while(!newScene.IsLoaded())
            {
                yield return null;
            }

            sceneLoader.UnloadScene(oldScene);

            m_transitionBackToView.Raise();
            yield return new WaitForSeconds(m_transitionDurationVar.Value);
        }
        #endregion

    }
}

