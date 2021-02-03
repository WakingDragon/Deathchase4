using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public class bullet_weaponAmmo : WeaponAmmo
    {
        private void Update()
        {
            if(m_isLaunched)
            {
                transform.Translate(Vector3.forward * m_ammoAsset.Speed());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var iDamageable = other.gameObject.GetComponent<IDamageable>();
            if(iDamageable != null)
            { 
                if(iDamageable.GetFaction() != m_faction)
                {
                    iDamageable.TakeDmg(m_ammoAsset.BaseDmg(), m_ammoAsset.DamageType());
                }
                Impact(transform.position);
            }
        }

        private void Impact(Vector3 worldPos)
        {
            if (m_impactFX) { m_impactFX.Play(worldPos, m_pool); }
            Unlaunch();
        }
    }
}

