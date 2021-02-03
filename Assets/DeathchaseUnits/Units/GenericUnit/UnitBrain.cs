using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Worlds.ProceduralTerrainGenerator;

namespace BP.Units
{
    public class UnitBrain : MonoBehaviour, IUnitState
    {
        private UnitStateType m_state;
        private UnitAsset m_asset;
        private bool m_isPlayer;
        private UnitMotor m_motor;

        //thoughts
        private TerrainVars_asset m_terrainVars;
        private GameObject m_targetIndicator;
        private Transform m_target;
        private float rangeForAction = 30f;
        [SerializeField] private bool m_activateOnEnable = true;
        private bool m_isActive;
        private float m_currentTurn, m_newTurn, m_angle;

        public void OnEnterNewUnitState(UnitStateType newState)
        {
            switch (newState)
            {
                case UnitStateType.assembled:
                    break;
                case UnitStateType.idling:
                    break;
                case UnitStateType.alive:
                    break;
                case UnitStateType.dead:
                    break;
                default:
                    break;
            }
            m_state = newState;
        }

        public void AssembleBrain(UnitStateType state, UnitMotor motor, UnitAsset asset)
        {
            m_state = state;
            m_motor = motor;
            m_asset = asset;

            m_isPlayer = m_asset.IsPlayer();
            m_terrainVars = m_asset.TerrainVars();
            m_targetIndicator = m_asset.TargetIndicator();
            m_currentTurn = 0f;
        }

        private void LateUpdate()
        {
            if (!m_isPlayer && m_state == UnitStateType.alive)
            {
                Think();
            }
        }

        private void OnDisable()
        {
            if (m_target) { Destroy(m_target.gameObject); }
        }

        private void Think()
        {
            if(!m_target)
            {
                //find a target - rnd point on map
                var targetLoc = m_terrainVars.GetRandomPointOnMap();
                m_target = Instantiate(m_targetIndicator).transform;
                m_target.transform.position = targetLoc;
            }

            if (!m_target) { Debug.Log("no target found"); return; }

            //rotate towards it
            m_angle = AngleToTarget();

            if(m_angle > 2f && m_angle <= 180f)
            {
                m_newTurn = Mathf.Lerp(m_currentTurn, 1f, Time.deltaTime);
            }
            else if (m_angle < 358f && m_angle > 180f)
            {
                m_newTurn = Mathf.Lerp(m_currentTurn, -1f, Time.deltaTime);
            }
            else
            {
                m_newTurn = Mathf.Lerp(m_currentTurn, 0f, Time.deltaTime);
            }
            m_motor.XAxis(m_newTurn);
            m_currentTurn = m_newTurn;

            //when within range of some action, you could raise an event or do an action
            if (Vector3.Distance(transform.position, m_target.position) < rangeForAction)
            {
                Destroy(m_target.gameObject);
            }
        }

        private float AngleToTarget()
        {
            var targetDir = m_target.transform.position - transform.position;
            var lookRot = Quaternion.FromToRotation(transform.forward, targetDir);
            return lookRot.eulerAngles.y;
        }
    }
}

