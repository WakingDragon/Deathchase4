using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="missile_ammoAsset",menuName ="Units/Ammo/Missile Ammo Asset")]
    public class missile_AmmoAsset : WeaponAmmoAsset
    {
        [SerializeField] private float m_lifespan = 2f;
        [SerializeField] private float m_scanRadius = 10f;
        [SerializeField] private float m_scanInterval = 0.5f;
        [SerializeField] private float m_turnSpeed = 2f;

        protected override void AssembleAmmo(GameObject go)
        {
            var wep = go.GetComponent<missile_weaponAmmo>();
            if (!wep) { wep = go.AddComponent<missile_weaponAmmo>(); }
            wep.SetupAmmo(this);
            wep.SetupMissile(m_lifespan, m_scanRadius, m_scanInterval, m_turnSpeed);
        }

        public float FuseTime() { return m_lifespan; }
    }
}

