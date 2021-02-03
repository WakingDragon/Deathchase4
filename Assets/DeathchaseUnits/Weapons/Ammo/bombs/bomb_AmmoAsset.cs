using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="bomb_ammoAsset",menuName ="Units/Ammo/Bomb Ammo Asset")]
    public class bomb_AmmoAsset : WeaponAmmoAsset
    {
        [SerializeField] private float m_fuseTime = 1f;
        [SerializeField] private float m_effectRadius = 10f;
        [SerializeField] private float m_effectDuration = 1f;

        protected override void AssembleAmmo(GameObject go)
        {
            var wep = go.GetComponent<bomb_weaponAmmo>();
            if (!wep) { wep = go.AddComponent<bomb_weaponAmmo>(); }
            wep.SetupAmmo(this);
            wep.SetTimerAndRadius(m_fuseTime, m_effectRadius, m_effectDuration);
        }

        public float FuseTime() { return m_fuseTime; }
    }
}

