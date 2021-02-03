using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Deathchase
{
    public class DCGameState : MonoBehaviour
    {
        [SerializeField] private bool m_activateOnStart = false;
        [SerializeField] private DCGameStateLibrary m_states = null;

        [Header("states")]
        [SerializeField] private DCGameStateAsset m_initialState = null;

        private GameSceneController m_controller;

        private void Awake()
        {
            if (!m_states) { Debug.Log("no state library"); }
            if (!m_initialState) { Debug.Log("no initial state"); }
        }

        private void Start()
        {
            if(m_activateOnStart) { EnterState(m_initialState); }
        }

        public void EnterState(DCGameStateAsset state)
        {
            m_states.EnterState(state);
        }
    }
}

