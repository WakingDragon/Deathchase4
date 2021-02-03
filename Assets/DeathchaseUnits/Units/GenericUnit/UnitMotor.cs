using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units
{
    public class UnitMotor : MonoBehaviour, IUnitState
    {
        private UnitStateType m_state;
        private UnitAsset m_asset;
        private float m_speed = 10f;
        private float m_rotSpeed = 10f;
        private float m_altitude = 2f;
        private Rigidbody m_rb;
        private UnitAnimation m_animation;

        private void LateUpdate()
        {
            if (m_state == UnitStateType.alive)
            {
                Move();
            }
        }

        #region states
        public void OnEnterNewUnitState(UnitStateType newState)
        {
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
            m_state = newState;
        }

        private void OnEnterIdleState()
        {
            m_rb.useGravity = false;
            m_rb.isKinematic = true;
        }

        private void OnEnterAliveState()
        {
            transform.position = new Vector3(transform.position.x, m_altitude, transform.position.z);
        }

        private void OnEnterDeadState()
        {
            m_rb.useGravity = true;
            m_rb.isKinematic = false;
        }
        #endregion

        #region assembly
        public void AssembleMotor(UnitStateType state, UnitAsset asset, Rigidbody rb, UnitAnimation animation)
        {
            m_state = state;
            m_asset = asset;

            m_speed = m_asset.Speed();
            m_rotSpeed = m_asset.RotSpeed();
            m_altitude = m_asset.Altitude();
            m_rb = rb;
            m_animation = animation;
        }
        #endregion

        private void Move()
        {
            transform.Translate(Vector3.forward * m_speed * Time.deltaTime, Space.Self);
        }

        public void XAxis(float x)
        {
            if (m_state != UnitStateType.alive) { return; }
            x = Mathf.Clamp(x, -1f, 1f);
            transform.Rotate(new Vector3(0f, m_rotSpeed * x * Time.deltaTime, 0f));
            m_animation.XAxis(x);
        }
        public void YAxis(float y)
        {
            //tbd
        }
        public void Fire(bool fire)
        {
            //tbd
        }
    }
}

