using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using BP.Worlds.ProceduralTerrainGenerator;
using System;
using BP.Units;

namespace BP.Deathchase
    {
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] private bool m_activateOnStart = false;
        [SerializeField] private int m_startingLives = 3;
        private int m_currentLives;
        private UnitPlacer m_unitPlacer;

        private enum DCCurrentState
        {
            loadingLevel,
            levelIntro,
            countdown,
            gameplay,
            pause,
            mainMenu,
            levelComplete,
            gameWon,
            gameLost
        }

        private DCCurrentState m_currentState;

        [Header("state notifications")]
        [SerializeField] private VoidGameEvent m_enterTransitionEvent = null;
        [SerializeField] private VoidGameEvent m_exitTransitionEvent = null;
        [SerializeField] private BoolGameEvent m_requestPauseGameEvent = null;
        [SerializeField] private DCLevelAssetGameEvent m_requestIntroUIEvent = null;
        [SerializeField] private BoolGameEvent m_pausePlayEvent = null;

        [Header("dependencies")]
        [SerializeField] private BoolGameEvent m_toggleMainMenu = null;
        [SerializeField] private VoidGameEvent m_onReadyToSetupLevel = null;
        [SerializeField] private VoidGameEvent m_closeLevelIntroEvent = null;
        [SerializeField] private VoidGameEvent m_startCountdownEvent = null;
        [SerializeField] private FloatVariable m_transitionDuration = null;
        private DCLevels m_levels;
        private TerrainGenerator m_terrainGen;
        //private DCGameState m_state;
        private bool m_blockInteractions;

        private void OnEnable()
        {
            if (m_activateOnStart)
            {
                TriggerSetup();
            }
        }

        public void TriggerSetup()
        {
            m_levels = GetComponent<DCLevels>();
            m_terrainGen = FindObjectOfType<TerrainGenerator>();
            if (!m_terrainGen) { Debug.Log("terrain generation not found by scene controller"); }
            //m_state = GetComponent<DCGameState>();
            //if (!m_state) { Debug.Log("state component not found by scene controller"); }
            m_unitPlacer = GetComponent<UnitPlacer>();
            if (!m_unitPlacer) { Debug.Log("unit placer component not found by scene controller"); }

            m_currentLives = m_startingLives;
            TriggerLoadLevel();
        }

        private void TriggerLoadLevel()
        {
            StartCoroutine(LoadFirstLevel());
        }

        private IEnumerator LoadFirstLevel()
        {
            //pause and transition
            m_currentState = DCCurrentState.loadingLevel;
            m_blockInteractions = true;
            m_pausePlayEvent.Raise(true);
            m_requestPauseGameEvent.Raise(true);
            m_enterTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);

            //load level
            m_levels.SetupFirstLevel(m_terrainGen);
            m_unitPlacer.SetLevelAsset(m_levels.CurrentLevel());
            yield return new WaitForSeconds(1f);
            m_exitTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);

            //show intro
            m_requestIntroUIEvent.Raise(m_levels.CurrentLevel());
            m_currentState = DCCurrentState.levelIntro;
            m_blockInteractions = false;
        }

        private IEnumerator LoadNextLevel()
        {
            //pause and transition
            m_currentState = DCCurrentState.loadingLevel;
            m_blockInteractions = true;
            m_pausePlayEvent.Raise(true);
            m_requestPauseGameEvent.Raise(true);
            m_enterTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);

            //load level
            m_levels.SetupNextLevel(m_terrainGen);
            m_unitPlacer.SetLevelAsset(m_levels.CurrentLevel());
            yield return new WaitForSeconds(1f);
            m_exitTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);

            //show intro
            m_requestIntroUIEvent.Raise(m_levels.CurrentLevel());
            m_currentState = DCCurrentState.levelIntro;
            m_blockInteractions = false;
        }

        public void OnAnyKeyDown()
        {
            if(m_currentState == DCCurrentState.levelIntro && !m_blockInteractions)
            {
                m_currentState = DCCurrentState.countdown;
                m_closeLevelIntroEvent.Raise();
                m_unitPlacer.PlaceUnits();
                m_startCountdownEvent.Raise();
            }

            if (!m_blockInteractions && (m_currentState == DCCurrentState.gameWon || m_currentState == DCCurrentState.gameLost))
            {
                //transitions and UI etc
                StartCoroutine(LoadFirstLevel());
                //reset scoring etc.
            }

            //only for debugging
            if (m_currentState == DCCurrentState.gameplay)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("1 pressed: go to win state");
                    OnGameWon();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Debug.Log("2 pressed: go to lose state");
                    OnGameLost();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Debug.Log("3 pressed: lost a life");    //this will be notified via a void event
                    OnLifeLost();
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    Debug.Log("4 pressed: level complete");    //this will be notified via a void event
                    OnLevelComplete();
                }
            }
        }

        public void OnCountdownFinished()
        {
            m_currentState = DCCurrentState.gameplay;
            m_blockInteractions = false;

            m_pausePlayEvent.Raise(false);
            //Debug.Log("playing");
        }

        public void OnGamePaused(bool isPaused)
        {
            //Debug.Log("pause=" + isPaused);
            if (isPaused)
            {
                m_currentState = DCCurrentState.pause;
                m_blockInteractions = true;
                m_pausePlayEvent.Raise(isPaused);
                Debug.Log("attempting to pause");
            }
            else
            {
                m_currentState = DCCurrentState.countdown;
                m_startCountdownEvent.Raise();
            }
        }

        public void OnLifeLost()
        {
            //notified that character has lost a life via voidlistener
            m_currentLives -= 1;
            if(m_currentLives < 0)
            {
                StartCoroutine(GameLost());
            }
            else
            {
                Debug.Log("lost a life! " + m_currentLives + " remaining");
                //place player
                //trigger countdown
            }
        }

        public void OnLevelComplete()
        {
            //notified that level is complete via voidlistener
            if (m_levels.IsCurrentLevelTheFinalLevel())
            {
                StartCoroutine(GameWon());
            }
            else
            {
                StartCoroutine(LoadNextLevel());
            }

        }

        public void OnGameWon()
        {
            StartCoroutine(GameWon());
        }

        private IEnumerator GameWon()
        {
            Debug.Log("game won!");
            m_currentState = DCCurrentState.gameWon;
            m_blockInteractions = true;
            m_pausePlayEvent.Raise(true);
            m_requestPauseGameEvent.Raise(true);
            m_enterTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);
            m_exitTransitionEvent.Raise();

            //show you won UI
            yield return new WaitForSeconds(m_transitionDuration.Value);
            m_blockInteractions = false;
        }

        public void OnGameLost()
        {
            StartCoroutine(GameLost());
        }

        private IEnumerator GameLost()
        {
            Debug.Log("game lost!");
            m_currentState = DCCurrentState.gameLost;
            m_blockInteractions = true;
            yield return new WaitForSeconds(2f);    //pause to allow death animations
            m_requestPauseGameEvent.Raise(true);
            m_pausePlayEvent.Raise(true);
            m_enterTransitionEvent.Raise();
            yield return new WaitForSeconds(m_transitionDuration.Value);
            m_exitTransitionEvent.Raise();

            //show you lost UI
            yield return new WaitForSeconds(m_transitionDuration.Value);
            m_blockInteractions = false;
        }
    }
}

