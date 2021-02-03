using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class TileActivation : MonoBehaviour
    {
        private Dictionary<Vector3, TerrainObjectInstance> items = new Dictionary<Vector3, TerrainObjectInstance>();
        private TerrainLevelAsset m_levelAsset;

        public void Activate()
        {
            ActivateItems();
            gameObject.SetActive(true);
        }

        private void ActivateItems()
        {
            if(items.Count < 1) { return; }

            foreach (KeyValuePair<Vector3, TerrainObjectInstance> item in items)
            {
                ActivateSingleItem(item.Value);
            }
        }

        private void ActivateSingleItem(TerrainObjectInstance item)
        {
            var go = m_levelAsset.GetFromPool(item);
            item.SetGO(go);
        }

        public void Populate(Vector3 worldPos, Vector3[] vertices, TerrainLevelAsset levelAsset)
        {
            m_levelAsset = levelAsset;
            if(!m_levelAsset) { Debug.Log("no level asset supplied"); return;  }

            items = m_levelAsset.PopulateVertices(worldPos,vertices,transform);
        }

        public void Deactivate()
        {
            DeactivateItems();
            gameObject.SetActive(false);
        }

        private void DeactivateItems()
        {
            foreach(KeyValuePair<Vector3,TerrainObjectInstance> item in items)
            {
                if(item.Value.GO() != null)
                {
                    m_levelAsset.RepoolObject(item.Value.GO());
                    item.Value.SetGO(null);
                }
            }
        }
    }
}

