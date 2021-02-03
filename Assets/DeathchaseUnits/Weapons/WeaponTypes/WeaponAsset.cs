using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;
using BP.ObjectPooling;
using System;

namespace BP.Units.Weapons
{
    public abstract class WeaponAsset : ScriptableObject
    {
        private ObjectPoolAsset m_pool = null; 

        [Header("poolables")]
        [SerializeField] private WeaponAmmoAsset m_ammoAsset = null;
        [SerializeField] private FXAsset m_launchFX = null;
        [SerializeField] private FXAsset m_impactFX = null;

        [Header("settings")]
        [SerializeField] private float m_fireInterval = 0.5f;

        public void PoolAmmoAndFX(ObjectPoolAsset pool)
        {
            m_pool = pool;
            m_pool.TryCreateNewPool(m_ammoAsset);
            if (m_launchFX) { m_pool.TryCreateNewPool(m_launchFX); }
            if (m_impactFX) { m_pool.TryCreateNewPool(m_impactFX); }
        }

        public virtual void Fire(Transform launchPoint, Faction faction)
        {
            //launch fx
            if (m_launchFX) { m_launchFX.Play(launchPoint, Vector3.zero, m_pool); }

            //launch ammo
            var go = m_pool.GetObjectFromPool(m_ammoAsset, launchPoint.position, launchPoint.rotation);
            var ammo = go.GetComponent<WeaponAmmo>();
            ammo.Launch(m_impactFX, m_pool, faction);
        }

        public void SetPoolAsset(ObjectPoolAsset poolAsset) { m_pool = poolAsset; }
        public float Interval() { return m_fireInterval; }
        protected WeaponAmmoAsset AmmoAsset() { return m_ammoAsset; }
    }
}

