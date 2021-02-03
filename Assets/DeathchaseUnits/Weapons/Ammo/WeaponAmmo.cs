using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public class WeaponAmmo : MonoBehaviour
    {
        protected ObjectPoolAsset m_pool;
        protected WeaponAmmoAsset m_ammoAsset;
        protected bool m_isLaunched = false;
        protected FXAsset m_impactFX;
        protected Faction m_faction;

        public void SetupAmmo(WeaponAmmoAsset ammoAsset)
        {
            m_ammoAsset = ammoAsset;
            if (!m_ammoAsset) { Debug.Log("no ammo asset on " + gameObject.name); }
        }

        private void OnDisable()
        {
            m_isLaunched = false;
        }

        public void Launch(FXAsset impactFX, ObjectPoolAsset pool, Faction faction)
        {
            m_pool = pool;
            m_impactFX = impactFX;
            m_faction = faction;
            m_isLaunched = true;
        }

        protected void Unlaunch()
        {
            if (m_isLaunched)
            {
                m_pool.Repool(gameObject);
                m_isLaunched = false;
            }
        }
    }
}

