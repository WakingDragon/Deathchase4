using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public class bomb_weaponAmmo : WeaponAmmo
    {
        [SerializeField] private float m_fuseTime;
        private float m_effectRadius;
        private float m_effectDuration;
        [SerializeField] private bool m_isExploded;
        private AmmoMesh m_bombMesh;
        private float m_fuseLeft;

        private void OnEnable()
        {
            ResetBomb();
        }

        public void SetTimerAndRadius(float fuseTime, float effectRadius, float effectDuration)
        {
            m_fuseTime = fuseTime;
            m_effectRadius = effectRadius;
            m_effectDuration = effectDuration;
            m_bombMesh = GetComponentInChildren<AmmoMesh>();

            ResetBomb();
        }

        private void ResetBomb()
        {
            m_bombMesh = GetComponentInChildren<AmmoMesh>();
            m_bombMesh.gameObject.SetActive(true);
            m_isExploded = false;
            m_fuseLeft = m_fuseTime;
        }

        private void Update()
        {
            if (m_isLaunched && m_fuseLeft <= 0f && !m_isExploded)
            {
                StartCoroutine(Explosion());
            }
            if (m_isLaunched && !m_isExploded) { m_fuseLeft -= Time.deltaTime; }
        }

        private IEnumerator Explosion()
        {
            m_isExploded = true;
            m_bombMesh.gameObject.SetActive(false);

            if (m_impactFX) 
            { 
                m_impactFX.Play(transform.position, m_pool); 
            }

            ZapEnemies();

            yield return new WaitForSeconds(m_effectDuration);

            m_bombMesh.gameObject.SetActive(true);
            Unlaunch();
        }

        private void ZapEnemies()
        {
            var enemies = m_faction.Library().GetAllEnemiesWithinRadius(transform.position, m_effectRadius, m_faction);

            if(enemies.Count > 0)
            {
                for(int i = 0; i < enemies.Count;i++)
                {
                    var iDamageable = enemies[i].GetComponent<IDamageable>();

                    if (iDamageable != null)
                    {
                        if (iDamageable.GetFaction() != m_faction)
                        {
                            iDamageable.TakeDmg(m_ammoAsset.BaseDmg(), m_ammoAsset.DamageType());
                        }
                    }
                }
            }
        }
    }
}

