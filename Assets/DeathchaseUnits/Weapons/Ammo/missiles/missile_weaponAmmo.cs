using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Units.Weapons
{
    public class missile_weaponAmmo : WeaponAmmo
    {
        [SerializeField] private float m_lifespan;
        [SerializeField] private float m_scanInterval;
        private float m_scanRadius;
        private float m_effectDuration;
        [SerializeField] private bool m_isExploded;
        private AmmoMesh m_ammoMesh;
        private float m_lifespanLeft;
        public Transform m_target;
        private bool m_huntingForTarget;
        private List<Transform> enemies;
        private Vector3 forwardPos = Vector3.zero;

        private void OnEnable()
        {
            ResetBomb();
        }

        public void SetupMissile(float lifespan, float scanRadius, float effectDuration, float scanInterval)
        {
            m_lifespan = lifespan;
            m_scanRadius = scanRadius;
            m_scanInterval = scanInterval;

            ResetBomb();
        }

        private void ResetBomb()
        {
            if (!m_ammoMesh) { m_ammoMesh = GetComponentInChildren<AmmoMesh>(); }
            m_ammoMesh.gameObject.SetActive(true);
            //m_isExploded = false;
            m_lifespanLeft = m_lifespan;
            m_huntingForTarget = false;
        }

        private void Update()
        {
            if (!m_isLaunched) { return; }
            if (m_target)
            {
                var realtivePos = m_target.position - transform.position;
                var lookRotation = Quaternion.LookRotation(realtivePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
            else
            {
                if (!m_huntingForTarget)
                {
                    StartCoroutine(Scan());
                }
            }
            transform.Translate(Vector3.forward * m_ammoAsset.Speed());
            CheckLifeSpan();
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
                //StartCoroutine(Explosion());
                Explode();
            }
        }

        private IEnumerator Scan()
        {
            m_huntingForTarget = true;

            yield return new WaitForSeconds(m_scanInterval);

            while (m_target == null)
            {
                //scan
                //find target and set it
                //set hunting as false
                forwardPos = transform.position + transform.forward * m_scanRadius;
                enemies = m_faction.Library().GetAllEnemiesWithinRadius(forwardPos, m_scanRadius, m_faction);
                
                if (enemies.Count > 0)
                {
                    m_target = enemies[0];
                    float bestDistance = Vector3.Distance(transform.position, enemies[0].position);
                    float newDistance;

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        newDistance = Vector3.Distance(transform.position, enemies[i].position);
                        if (newDistance < bestDistance)
                        {
                            m_target = enemies[i];
                            bestDistance = newDistance;
                            Debug.Log("target found");
                        }
                    }
                }
            }
            if (m_target != null) { m_huntingForTarget = false; }
        }

        private void Explode()
        {
            m_impactFX.Play(transform.position, m_pool);
            Unlaunch();
        }

        private IEnumerator Explosion()
        {
            m_isExploded = true;
            if (m_ammoMesh) { m_ammoMesh.gameObject.SetActive(false); }

            if (m_impactFX) 
            { 
                
            }

            //ZapEnemies();

            yield return new WaitForSeconds(m_effectDuration);

            m_ammoMesh.gameObject.SetActive(true);
            Unlaunch();
        }

        private void CheckLifeSpan()
        {
            if (m_lifespanLeft <= 0f)
            {
                //StartCoroutine(Explosion());
                Explode();
            }
            else
            {
                m_lifespanLeft -= Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(forwardPos, m_scanRadius);
        }
    }
}

