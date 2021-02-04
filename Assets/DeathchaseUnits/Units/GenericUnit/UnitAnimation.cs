using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Units.Weapons;
using BP.ObjectPooling;
using BP.Core;

namespace BP.Units
    {
    public class UnitAnimation : MonoBehaviour, IUnitState
    {
        private UnitStateType m_state;
        private UnitAsset m_asset;
        private GameObject m_mesh;
        private bool m_isSetup;
        private ObjectPoolAsset m_pool;
        private FXAsset m_deathFX;
        private FXAsset m_collisionFX;
        private VoidGameEvent m_camShakeEvent;
        public BoolGameEvent m_togglePanWideOffsetEvent;    //move to asset
        public GameObject m_smokeFX;    //maybe just make part of the model like lights
        private bool m_hasHighDamage = false;
        private AudioCue m_motorSFX;

        public void AssembleAnimation(UnitStateType state, UnitAsset asset)
        {
            m_asset = asset;
            m_state = state;
            m_pool = m_asset.ObjectPoolAsset();
            m_deathFX = m_asset.DeathFX();
            m_collisionFX = m_asset.CollisionFX();
            m_camShakeEvent = m_asset.CamShakeEvent();
            m_motorSFX = m_asset.MotorSFX();

            m_mesh = m_asset.AddMeshToUnit(transform);

        }

        #region states
        public void OnEnterNewUnitState(UnitStateType newState)
        {
            switch (newState)
            {
                case UnitStateType.assembled:
                    OnReEnterAssembledState();
                    break;
                case UnitStateType.idling:
                    OnEnterIdleState();
                    break;
                case UnitStateType.alive:
                    OnEnterPlayState();
                    break;
                case UnitStateType.dead:
                    OnEnterDeadState();
                    break;
                default:
                    break;
            }
            m_state = newState;
        }

        private void OnReEnterAssembledState()
        {
            m_mesh.SetActive(true);
        }

        private void OnEnterIdleState()
        {
            m_pool.TryCreateNewPool(m_deathFX);
            if (m_asset.IsPlayer()) { m_togglePanWideOffsetEvent.Raise(true); }
            m_smokeFX.SetActive(false);
        }

        private void OnEnterPlayState()
        {
            if (m_asset.IsPlayer()) { m_togglePanWideOffsetEvent.Raise(false); }
            if (m_motorSFX) { m_motorSFX.Play(); }
        }

        private void OnEnterDeadState()
        {
            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {
            if (m_deathFX) { m_deathFX.Play(transform, Vector3.zero, m_pool); }
            m_camShakeEvent.Raise();
            if (m_asset.IsPlayer()) { m_togglePanWideOffsetEvent.Raise(true); }

            yield return new WaitForSeconds(3f);
        }
        #endregion

        #region triggered tasks
        public void XAxis(float x)
        {
            float tilt = 20f;
            var rot = new Vector3(
                m_mesh.transform.localRotation.x,
                m_mesh.transform.localRotation.y,
                m_mesh.transform.localRotation.z - (tilt * x)
                );
            m_mesh.transform.localRotation = Quaternion.Euler(rot);
        }

        public void ImpactFX(DamageType dmgType)
        {
            if (dmgType.DefaultDmgFX() != null)
            {
                dmgType.DefaultDmgFX().Play(transform, Vector3.zero, m_pool);
            }
            if (m_asset.IsPlayer()) { m_camShakeEvent.Raise(); }
        }

        public void HighDamage(bool isHighDamage)
        {
            if(isHighDamage == m_hasHighDamage) { return; }

            if (isHighDamage)
            {
                m_smokeFX.SetActive(true);
            }
            else
            {
                m_smokeFX.SetActive(false);
            }
            m_hasHighDamage = isHighDamage;
        }

        public void CollisionAnimation()
        {
            m_collisionFX.Play(transform.position, m_pool);
            m_camShakeEvent.Raise();
            transform.position = transform.position - (5f * transform.forward);
        }
        #endregion
    }
}

