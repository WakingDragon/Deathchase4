using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public class UnitWeapons : MonoBehaviour, IUnitState
    {
        private UnitStateType m_state;
        private bool m_autoFire;
        private bool m_isFiring = false;
        //[SerializeField] private bool m_activateOnEnable;
        private List<WeaponSlot> m_weaponSlots;
        private float m_timeForNextFire;
        private UnitAsset m_asset;
        private Faction m_faction;
        private bool m_weaponsReady;
        private ObjectPoolAsset m_pool;
        //private bool m_isPaused = true;

        #region assembly
        public void SetupUnitWeapons(UnitStateType state, UnitAsset asset)
        {
            m_asset = asset;
            m_state = state;

            m_faction = m_asset.Faction();
            m_pool = m_asset.ObjectPoolAsset();

            m_weaponSlots = m_asset.SetWeaponSlotsOnUnit(transform);

            m_autoFire = m_asset.AutoFire();
            
            InitialiseAllWeaponSlots();
            //m_activateOnEnable = activateOnEnable;
            //ConfigForStart();
        }

        private void InitialiseAllWeaponSlots()
        {
            foreach (WeaponSlot weaponSlot in m_weaponSlots)
            {
                weaponSlot.Initialise(m_pool, m_faction);
                weaponSlot.EquipFirstWeapon();
            }
            m_weaponsReady = true;
        }
        #endregion

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
            m_isFiring = false;
        }

        private void OnEnterAliveState()
        {
            if (m_autoFire) { m_isFiring = true; }
            m_timeForNextFire = Time.time;
        }

        private void OnEnterDeadState()
        {
            m_isFiring = false;
        }
        #endregion

        //private void OnEnable()
        //{
        //    ConfigForStart();
        //}

        //private void ConfigForStart()
        //{
        //    m_timeForNextFire = Time.time;
        //    m_isPaused = true;

        //    if (m_activateOnEnable)
        //    {
        //        m_autoFire = true;
        //        m_isFiring = true;
        //    }
        //}

        private void Update()
        {
            if(m_weaponsReady && Time.time > m_timeForNextFire && m_isFiring)
            {
                CycleThroughWeaponSlots();
            }
        }

        //public void OnGamePause(bool isPaused)
        //{
        //    m_isPaused = isPaused;
        //}

        private void CycleThroughWeaponSlots()
        {

            foreach (WeaponSlot weaponSlot in m_weaponSlots)
            {
                if(weaponSlot.IsLoaded())
                {
                    //Debug.Log("triggering for " + gameObject.name);
                    var nextFireTime = weaponSlot.TryFireWeapon();
                    if(nextFireTime < m_timeForNextFire)
                    {
                        m_timeForNextFire = nextFireTime;
                    }
                }
            }
        }
    }
}

