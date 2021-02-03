using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Worlds.ProceduralTerrainGenerator;
using BP.ObjectPooling;

namespace BP.Deathchase
{
    public class DCLevels : MonoBehaviour
    {
        [SerializeField] private DCLevelAsset[] m_levels;

        [Header("dependencies")]
        [SerializeField] private ObjectPoolAsset m_pool;
        private int m_currentLevelIndex;

        private void Awake()
        {
            if (m_levels.Length < 1) { Debug.Log("no levels"); }
            if (!m_pool) { Debug.Log("no pool"); }

            foreach (DCLevelAsset level in m_levels)
            {
                level.SetPoolAsset(m_pool);
            }
        }

        public void SetupFirstLevel(TerrainGenerator terrainGen)
        {
            terrainGen.DestroyTerrain();
            m_currentLevelIndex = 0;
            terrainGen.SetNewTerrainLevelAsset(m_levels[m_currentLevelIndex]);
            terrainGen.Activate();
        }

        public void SetupNextLevel(TerrainGenerator terrainGen)
        {
            terrainGen.DestroyTerrain();
            m_currentLevelIndex += 1;
            terrainGen.SetNewTerrainLevelAsset(m_levels[m_currentLevelIndex]);
            terrainGen.Activate();
        }

        public DCLevelAsset CurrentLevel()
        {
            return m_levels[m_currentLevelIndex];
        }

        public bool IsCurrentLevelTheFinalLevel()
        {
            if (m_levels.Length-1 == m_currentLevelIndex)
            {
                return true;
            }
            return false;
        }
    }
}

