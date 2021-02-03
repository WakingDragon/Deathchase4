using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public abstract class WeaponAmmoAsset : ScriptableObject, IPoolableAsset
    {
        [SerializeField] private GameObject m_prefab = null;

        [Header("Pooling settings")]
        [SerializeField] private int m_defaultNumberToPool = 5;
        [SerializeField] private bool m_expandablePool = true;
        [SerializeField] private float m_autoRepoolLife = 2f;

        [Header("game specific settings")]
        [SerializeField] private float m_ammoSpeed = 10f;
        [SerializeField] private DamageType m_damageType = null;
        [SerializeField] private float m_baseDamage = 10f;

        #region IPoolable
        public string GetName()
        {
            return this.name.ToString();
        }
        public IPoolableAsset GetIPoolableAsset() { return this; }
        public int GetNumberToPool() { return m_defaultNumberToPool; }
        public bool GetIsPoolExpandable() { return m_expandablePool; }

        public GameObject InstantiatePrefabToPool(Transform parent)
        {
            //This can be used to put in very unique elements to pooled object spawning
            GameObject go = Instantiate(m_prefab, parent);
            AssembleAmmo(go);
            return go;
        }

        public float AutoRepool()
        {
            return m_autoRepoolLife;
        }
        #endregion

        #region ammo specific
        protected virtual void AssembleAmmo(GameObject go) { }
        public virtual void ResetAmmo(GameObject go) { }

        public float Speed() { return m_ammoSpeed; }
        public float BaseDmg() { return m_baseDamage; }
        public DamageType DamageType() { return m_damageType; }
        #endregion
    }
}

