using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Units.Weapons;
using BP.Core;
using BP.Deathchase;
using System;

namespace BP.Units
{
    public class UnitBuilder : MonoBehaviour
    {
        public UnitStateType m_state = UnitStateType.none;

        [SerializeField] private bool m_runOnAwake = false;
        [SerializeField] private UnitAsset m_asset = null;
        private bool m_isPausedInGameplay;
        private bool m_isSetup;
        private bool m_isActivated;

        private bool m_isPlayer;
        private UnitBrain m_brain;
        private UnitHealth m_health;
        private UnitMotor m_motor;
        private UnitWeapons m_weapons;
        private Rigidbody m_rb;
        private BoxCollider m_collider;
        private UnitAnimation m_animation;
        private Faction m_faction;

        [Header("dependencies")]
        //private Transform_listSet m_npcList;
        [SerializeField] private TransformGameEvent m_playerOnPitchGameEvent = null;
        [SerializeField] private GameObject m_cameraArmPrefab = null;
        [SerializeField] private UnityBoolListener m_fireListener = null;
        [SerializeField] private UnityFloatListener m_xAxisListener = null;
        [SerializeField] private UnityFloatListener m_yAxisListener = null;

        private List<IUnitState> componentStates = new List<IUnitState>();

        #region state management
        public void SetNewUnitState(UnitStateType newState)
        {
            m_state = newState;

            switch (newState)
            {
                case UnitStateType.assembled:
                    break;
                case UnitStateType.idling:
                    OnEnterIdleState();
                    break;
                case UnitStateType.alive:
                    OnEnterAliveState();
                    break;
                case UnitStateType.dead:
                    OnEnterDeadState();
                    break;
                default:
                    break;
            }

            foreach(IUnitState componentState in componentStates)
            {
                componentState.OnEnterNewUnitState(newState);
            }
        }
        #endregion

        #region assembly
        public void AssembleUnit(UnitAsset asset, bool gotoIdling, bool gotoAlive)
        {
            //has been spawned by asset
            if (!asset) { Debug.Log("no asset supplied to builder"); }
            m_state = UnitStateType.none;
            m_asset = asset;
            m_isPlayer = m_asset.IsPlayer();
            m_faction = m_asset.Faction();

            GetComponents();
            AssembleSubComponents();

            SetNewUnitState(UnitStateType.assembled);

            if(gotoIdling || gotoAlive) { SetNewUnitState(UnitStateType.idling); }
            if (gotoAlive) { SetNewUnitState(UnitStateType.alive); }
        }

        private void AssembleSubComponents()
        {
            m_asset.SetColliderAndRBOnUnit(m_rb, m_collider);

            m_brain.AssembleBrain(m_state, m_motor, m_asset);

            m_motor.AssembleMotor(m_state, m_asset, m_rb, m_animation);

            m_health.AssembleHealth(m_state, m_asset, this, m_animation);

            if(m_isPlayer)
            {
                CreateCameraArm();
            }

            m_weapons.enabled = true;
            m_weapons.SetupUnitWeapons(m_state,m_asset);

            m_fireListener.enabled = m_isPlayer;
            m_xAxisListener.enabled = m_isPlayer;
            m_yAxisListener.enabled = m_isPlayer;

            m_animation.AssembleAnimation(m_state, m_asset);
        }

        private void GetComponents()
        {
            m_collider = GetComponent<BoxCollider>();
            if (!m_collider)
            {
                Debug.Log("no collider on " + gameObject.name);
                m_collider = gameObject.AddComponent<BoxCollider>();
            }

            m_rb = GetComponent<Rigidbody>();
            if (!m_rb)
            {
                Debug.Log("no rb on " + gameObject.name);
                m_rb = gameObject.AddComponent<Rigidbody>();
            }

            m_brain = GetComponent<UnitBrain>();
            if (!m_brain)
            {
                Debug.Log("no brains on " + gameObject.name);
                m_brain = gameObject.AddComponent<UnitBrain>();
            }
            componentStates.Add(m_brain);

            m_motor = GetComponent<UnitMotor>();
            if (!m_motor)
            {
                Debug.Log("no motor on " + gameObject.name);
                m_motor = gameObject.AddComponent<UnitMotor>();
            }
            componentStates.Add(m_motor);

            m_health = GetComponent<UnitHealth>();
            if (!m_health)
            {
                Debug.Log("no health on " + gameObject.name);
                m_health = gameObject.AddComponent<UnitHealth>();
            }
            componentStates.Add(m_health);

            m_weapons = GetComponent<UnitWeapons>();
            if (!m_weapons)
            {
                Debug.Log("no weps on " + gameObject.name);
                m_weapons = gameObject.AddComponent<UnitWeapons>();
            }
            componentStates.Add(m_weapons);

            m_animation = GetComponent<UnitAnimation>();
            if (!m_animation)
            {
                Debug.Log("no animator on " + gameObject.name);
                m_animation = gameObject.AddComponent<UnitAnimation>();
            }
            componentStates.Add(m_animation);
        }
        #endregion

        #region idling
        private void OnEnterIdleState()
        {
            m_faction.AddMeToLibrary(gameObject.transform);
            
            if (m_isPlayer)
            {
                m_playerOnPitchGameEvent.Raise(transform);
            }
            else
            {
                m_asset.NPCListSet().AddToList(transform);
            }
        }
        #endregion

        #region alive state
        private void OnEnterAliveState()
        {

        }
        #endregion

        #region dead
        private void OnEnterDeadState()
        {
            if (!m_isPlayer)
            {
                m_asset.NPCListSet().RemoveFromList(transform);
                if (m_asset.NPCListSet().Count() < 1) { m_asset.NotifyAllBaddiesDead(); }
            }
            m_faction.RemoveMeFromLibrary(gameObject.transform);
        }
        #endregion


        private void OnEnable()
        {
            if(m_state == UnitStateType.assembled)
            {
                SetNewUnitState(UnitStateType.idling);
            }
        }

        //private void ActivateBuilder()
        //{
        //    if(m_isSetup)
        //    {
        //        //if (!m_isPlayer && m_npcList) { m_npcList.AddToList(transform); }
        //        //m_motor.SetMoving(true);
        //        m_isActivated = true;
        //    }
        //}

        //private void OnDisable()
        //{
        //    //if (!m_isPlayer && m_npcList) { m_npcList.RemoveFromList(transform); }
        //    //m_isActivated = false;
        //}

        #region builder tasks
        private void CreateCameraArm()
        {
            var arm = GetComponentInChildren<CameraArm>();
            if (!arm) { Instantiate(m_cameraArmPrefab, transform); }
        }

        public void OnUnitDies()
        {
            SetNewUnitState(UnitStateType.dead);
        }
        #endregion
    }
}


