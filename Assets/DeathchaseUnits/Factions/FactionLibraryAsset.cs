using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units
{
    [CreateAssetMenu(fileName = "factionLibrary", menuName = "Units/Factions/(single) Faction Library")]
    public class FactionLibraryAsset : ScriptableObject
    {
        [SerializeField] private Faction[] m_factions;
        private Dictionary<Faction, List<Transform>> m_library = new Dictionary<Faction, List<Transform>>();

        private void OnEnable()
        {
            if(m_factions.Length < 1) { return;  }

            foreach(Faction faction in m_factions)
            {
                faction.SetLibrary(this);

                if (!m_library.ContainsKey(faction))
                {
                    m_library.Add(faction, new List<Transform>());
                }
            }
        }

        public void AddTransform(Transform t, Faction faction)
        {
            if(!m_library[faction].Contains(t)) { m_library[faction].Add(t); }
        }

        public void RemoveTransform(Transform t, Faction faction)
        {
            if (m_library[faction].Contains(t)) { m_library[faction].Remove(t); }
        }

        #region tools
        public List<Transform> GetAllEnemiesWithinRadius(Vector3 worldOrigin, float radius, Faction myFaction)
        {
            List<Transform> newList = new List<Transform>();

            foreach(Faction faction in m_factions)
            {
                if(faction != myFaction && m_library.ContainsKey(faction))
                {
                    foreach(Transform t in m_library[faction])
                    {
                        if(t != null && Vector3.Distance(worldOrigin,t.position) < radius)
                        {
                            newList.Add(t);
                        }
                    }
                }
            }
            return newList;
        }
        #endregion
    }
}

