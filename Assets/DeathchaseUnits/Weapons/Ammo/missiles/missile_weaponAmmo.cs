using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;
using BP.Core;

namespace BP.Units.Weapons
{
    public class missile_weaponAmmo : WeaponAmmo
    {
        private float m_lifespan;
        private float m_scanInterval;
        private float m_scanRadius;
        private bool m_isExploded;
        private AmmoMesh m_ammoMesh;
        private float m_lifespanLeft;
        private Transform m_target;
        private bool m_scanning;
        private List<Transform> enemies = new List<Transform>();
        private Quaternion lookRotation;
        private float m_turnSpeed;

        private void OnEnable()
        {
            StopAllCoroutines();
            ResetBomb();
        }

        public void SetupMissile(float lifespan, float scanRadius, float scanInterval, float turnSpeed)
        {
            m_lifespan = lifespan;
            m_scanRadius = scanRadius;
            m_scanInterval = scanInterval;
            m_turnSpeed = turnSpeed;

            ResetBomb();
        }

        private void ResetBomb()
        {
            if (!m_ammoMesh) { m_ammoMesh = GetComponentInChildren<AmmoMesh>(); }
            m_ammoMesh.gameObject.SetActive(true);
            m_isExploded = false;
            m_lifespanLeft = m_lifespan;
            m_scanning = false;
        }

        private void Update()
        {
            if (!m_isLaunched || m_isExploded) { return; }
            CheckLifeSpan();

            transform.Translate(Vector3.forward * m_ammoAsset.Speed());

            if(m_target)
            {
                transform.rotation = CalculateRotationToTarget(m_target);
            }

            if(!m_target && !m_scanning && !m_isExploded)
            {
                StartCoroutine(Scanning());
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnTriggerEnter(Collider other)
        {
            var iDamageable = other.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                if (iDamageable.GetFaction() != m_faction)
                {
                    iDamageable.TakeDmg(m_ammoAsset.BaseDmg(), m_ammoAsset.DamageType());
                }
                Explode();
            }

            var iCollidable = other.gameObject.GetComponent<ICollidable>();
            if (iCollidable != null)
            {
                Explode();
            }
        }

        private Quaternion CalculateRotationToTarget(Transform target)
        {
            if (!target) { return Quaternion.identity; }

            lookRotation = Quaternion.LookRotation(target.position - transform.position);
            return Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * m_turnSpeed);
        }

        private IEnumerator Scanning()
        {
            m_scanning = true;

            while(m_target == null)
            {
                m_target = ScanForTarget(CalculateScanForwardOrigin());
                yield return new WaitForSeconds(m_scanInterval);
            }

            m_scanning = false;
        }

        private Transform ScanForTarget(Vector3 scanOrigin)
        {
            enemies.Clear();
            enemies = m_faction.Library().GetAllEnemiesWithinRadius(scanOrigin, m_scanRadius, m_faction);

            if (enemies.Count > 0)
            {
                Transform m_potentialTarget = enemies[0];
                float bestDistance = Vector3.Distance(transform.position, enemies[0].position);
                float newDistance;

                for (int i = 0; i < enemies.Count; i++)
                {
                    newDistance = Vector3.Distance(transform.position, enemies[i].position);
                    if (newDistance < bestDistance)
                    {
                        m_potentialTarget = enemies[i];
                        bestDistance = newDistance;
                        
                    }
                }
                return m_potentialTarget;
            }
            else
            {
                return null;
            }
        }

        private Vector3 CalculateScanForwardOrigin()
        {
            return transform.position + transform.forward * m_scanRadius;
        }

        private void CheckLifeSpan()
        {
            if (m_lifespanLeft <= 0f)
            {
                Explode();
            }
            else
            {
                m_lifespanLeft -= Time.deltaTime;
            }
        }

        private void Explode()
        {
            m_isExploded = true;
            m_impactFX.Play(transform.position, m_pool);
            m_target = null;
            Unlaunch();
        }
    }
}

