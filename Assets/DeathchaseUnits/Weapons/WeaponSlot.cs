using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public enum WeaponSlotType { main, wings, bomber }

    [System.Serializable]
    public class WeaponSlotItem
    {
        public Vector3 location;
        public WeaponSlotType slotType;
        public WeaponAsset defaultWeapon;
    }

    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private WeaponAsset m_weaponAsset = null;
        [SerializeField] private WeaponSlotType m_slotType = WeaponSlotType.main;
        private float m_timeForNextFire;
        private ObjectPoolAsset m_pool;
        private bool m_isLoaded = false;
        private Faction m_faction;

        public bool IsLoaded() { return m_isLoaded; }

        public void DefineSlot(WeaponSlotType slotType, WeaponAsset defaultWeaponAsset)
        {
            m_slotType = slotType;
            if(defaultWeaponAsset != null) { m_weaponAsset = defaultWeaponAsset; }
        }

        public void Initialise(ObjectPoolAsset pool, Faction faction)
        {
            m_pool = pool;
            m_faction = faction;
        }

        public void EquipFirstWeapon()
        {
            if (m_weaponAsset)
            {
                EquipNewWeapon(m_weaponAsset);
            }
        }

        public void EquipNewWeapon(WeaponAsset weaponAsset)
        {
            m_weaponAsset = weaponAsset;
            if (m_weaponAsset)
            {
                m_weaponAsset.PoolAmmoAndFX(m_pool);
                m_timeForNextFire = m_weaponAsset.Interval();
                m_isLoaded = true;
            }
            else
            {
                Debug.Log("no weapon asset to equip");
            }
        }

        public float TryFireWeapon()
        {
            if(m_weaponAsset && Time.time > m_timeForNextFire)
            {
                m_weaponAsset.Fire(transform, m_faction);
                m_timeForNextFire = Time.time + m_weaponAsset.Interval();
            }
            return m_timeForNextFire;
        }
    }
}

