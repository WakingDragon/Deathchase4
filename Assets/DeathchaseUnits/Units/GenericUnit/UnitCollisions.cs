using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Units.Weapons;
using BP.ObjectPooling;
using BP.Core;

namespace BP.Units
{
    public class UnitCollisions : MonoBehaviour, IUnitState
    {
        private UnitStateType m_state;
        private UnitAsset m_asset;
        private UnitBuilder m_builder;
        private Rigidbody m_rb;
        private BoxCollider m_collider;
        private bool m_detectCollisions = false;

        #region collisions
        private void OnCollisionEnter(Collision collision)
        {
            if(!m_detectCollisions) { return; }

            var iCollidable = collision.gameObject.GetComponent<ICollidable>();
            if(iCollidable != null)
            {
                   m_builder.DoCollision();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_detectCollisions) { return; }

            var iCollidable = other.gameObject.GetComponent<ICollidable>();
            if (iCollidable != null)
            {
                StartCoroutine(m_builder.DoCollision());
            }
        }

        public void ToggleCollisionDetection(bool detectCollisions)
        {
            m_detectCollisions = detectCollisions;
        }
        #endregion

        public void AssembleCollisions(UnitStateType state, UnitAsset asset, UnitBuilder builder, Rigidbody rb, BoxCollider collider)
        {
            m_asset = asset;
            m_state = state;
            m_builder = builder;
            m_rb = rb;
            m_collider = collider;
        }

        #region states
        public void OnEnterNewUnitState(UnitStateType newState)
        {
            switch (newState)
            {
                case UnitStateType.assembled:
                    break;
                case UnitStateType.idling:
                    OnEnterIdlingState();
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

        private void OnEnterIdlingState()
        {
            m_rb.useGravity = false;
            m_rb.isKinematic = m_asset.IsKinematic();
            m_collider.isTrigger = m_asset.IsTrigger();
            m_detectCollisions = false;
        }

        private void OnEnterAliveState()
        {
            if (m_asset.CanHitCollidables()) { m_detectCollisions = true; }
        }

        private void OnEnterDeadState()
        {
            m_rb.useGravity = true;
            m_rb.isKinematic = false;
            m_collider.isTrigger = false;
            m_detectCollisions = false;
        }
        #endregion
    }
}