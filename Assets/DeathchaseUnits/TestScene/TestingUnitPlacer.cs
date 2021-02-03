using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Deathchase;
using BP.Units.Weapons;

namespace BP.Units
{
    public class TestingUnitPlacer : MonoBehaviour
    {
        //[SerializeField] private GameObject m_unitPrefab = null;
        [SerializeField] private DCLevelAsset m_levelAsset = null;
        [SerializeField] private DamageType m_dmgType = null;

        public void SetLevelAsset(DCLevelAsset levelAsset)
        {
            m_levelAsset = levelAsset;
        }

        public void CheckKeyPress()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {

                PlaceUnits();
            }
        }

        public void PlaceUnits()
        {
            StartCoroutine(Lifespan());
        }

        private IEnumerator Lifespan()
        {
            yield return new WaitForSeconds(1f);

            var spawnpoint = Vector3.zero;
            var baddies = m_levelAsset.Baddies();

            UnitBuilder builder;
            
            foreach(UnitAsset baddie in baddies)
            {
                builder = baddie.SpawnAndAssembleUnitAt(spawnpoint, true, false);
                builder.SetNewUnitState(UnitStateType.alive);
            }

            yield return new WaitForSeconds(1f);

            builder = m_levelAsset.Player().SpawnAndAssembleUnitAt(spawnpoint, true, false);
            var health = builder.GetComponent<UnitHealth>();

            yield return new WaitForSeconds(0.2f);

            builder.SetNewUnitState(UnitStateType.alive);

            //yield return new WaitForSeconds(2f);

            //float dmgInterval = 0.5f;
            //float dmg = 20f;

            //health.TakeDmg(dmg, m_dmgType);
            //yield return new WaitForSeconds(dmgInterval);
            //health.TakeDmg(dmg, m_dmgType);
            //yield return new WaitForSeconds(dmgInterval);
            //health.TakeDmg(dmg, m_dmgType);
            //yield return new WaitForSeconds(dmgInterval);
            //health.TakeDmg(dmg, m_dmgType);

            //yield return new WaitForSeconds(2f);
        }
    }
}

