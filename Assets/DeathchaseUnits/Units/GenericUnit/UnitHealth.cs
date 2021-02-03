using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Units.Weapons;
using BP.ObjectPooling;
using BP.Core;

namespace BP.Units
{
    public class UnitHealth : MonoBehaviour, IDamageable, IUnitState
    {
        private UnitStateType m_state;
        private UnitAsset m_asset;
        private UnitBuilder m_builder;
        private UnitAnimation m_animation;
        private bool m_isPlayer;
        private Faction m_faction;
        //private FXAsset m_deathFX;
        private ObjectPoolAsset m_pool;
        //private VoidGameEvent m_camShakeEvent;
        private Vector2GameEvent m_playerHealthChangeEvent;
        private VoidGameEvent m_playerDeathEvent;
        private float m_currentHealth;

        public void AssembleHealth(UnitStateType state, UnitAsset asset, UnitBuilder builder, UnitAnimation animation)
        {
            m_asset = asset;
            m_state = state;
            m_builder = builder;
            m_animation = animation;

            m_isPlayer = m_asset.IsPlayer();
            m_pool = m_asset.ObjectPoolAsset();
            m_faction = m_asset.Faction();

            //m_camShakeEvent = m_asset.CamShakeEvent();
            m_playerHealthChangeEvent = m_asset.UnitHealthChangeEvent();
            m_playerDeathEvent = m_asset.UnitDeathEvent();

            //m_deathFX = m_asset.DeathFX();
            //m_pool.TryCreateNewPool(m_deathFX);
        }

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
                    break;
                case UnitStateType.dead:
                    break;
                default:
                    break;
            }
            m_state = newState;
        }

        private void OnEnterIdleState()
        {
            m_currentHealth = m_asset.StartingHealth();
            NotifyHealthChange();
        }

        #region iDamageable
        public Faction GetFaction() { return m_faction; }

        public void TakeDmg(float dmg, DamageType dmgType)
        {
            if(m_state == UnitStateType.alive)
            {
                m_currentHealth -= dmg;
                NotifyHealthChange();
                m_animation.ImpactFX(dmgType);
                if (m_currentHealth <= 0f) { m_builder.OnUnitDies(); }

                if(Utils.ClampedPercent(m_currentHealth,m_asset.StartingHealth()) < 0.3f)
                {
                    m_animation.HighDamage(true);
                }
            }
        }
        #endregion

        private void NotifyHealthChange()
        {
            if (m_isPlayer)
            {
                m_playerHealthChangeEvent.Raise(new Vector2(m_currentHealth, m_asset.StartingHealth()));
            }
        }
    }
}