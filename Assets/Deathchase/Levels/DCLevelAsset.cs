using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using BP.ObjectPooling;
using BP.Worlds.ProceduralTerrainGenerator;
using BP.Units;

namespace BP.Deathchase
{
    [CreateAssetMenu(menuName = "Deathchase/New Level Asset", fileName = "new_dcLevel")]
    public class DCLevelAsset : TerrainLevelAsset
    {
        [Header("Level Basics")]
        [SerializeField] private string m_levelName = "blank level";

        [Header("characters")]
        [SerializeField] private UnitAsset m_player = null;
        [SerializeField] private UnitAsset[] m_baddies;

        public string LevelName() { return m_levelName; }

        public UnitAsset Player() { return m_player; }
        public UnitAsset[] Baddies() { return m_baddies; }
    }
}


