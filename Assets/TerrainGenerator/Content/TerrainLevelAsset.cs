using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using BP.ObjectPooling;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    [System.Serializable]
    public class TerrainEnvironmentItem
    {
        public TerrainObjectAsset itemAsset;
        public Vector2Int numToSpawn;
    }

    [CreateAssetMenu(menuName = "Terrain Generator/Terrain Level Asset", fileName = "newLevel_levelAsset")]
    public class TerrainLevelAsset : ScriptableObject
    {
        [Header("Level Basics")]
        [SerializeField] private Material groundMat = null;
        [SerializeField] private ObjectPoolAsset objectPool = null;

        [Header("Spawned Per Tile")]
        [SerializeField] private List<TerrainEnvironmentItem> perTileItems = new List<TerrainEnvironmentItem>();

        [Header("Spawned across world")]
        [SerializeField] private List<TerrainEnvironmentItem> worldEnvironmentItems = new List<TerrainEnvironmentItem>();

        #region setup
        public void SetPoolAsset(ObjectPoolAsset poolAsset) { objectPool = poolAsset; }
        #endregion

        #region creating content
        public void PoolTerrainObjects()
        {
            foreach(TerrainEnvironmentItem item in perTileItems)
            {
                if(item.itemAsset != null)
                {
                    objectPool.TryCreateNewPool(item.itemAsset);
                }
            }
        }

        public void DestroyTerrainObjectPools()
        {
            foreach (TerrainEnvironmentItem item in perTileItems)
            {
                if (item.itemAsset != null)
                {
                    objectPool.DeleteObjectPoolBySceneName(item.itemAsset.name.ToString());
                }
            }
        }

        public void RepoolObject(GameObject go)
        {
            objectPool.Repool(go);
        }

        public Dictionary<Vector3, TerrainObjectInstance> PopulateVertices(Vector3 worldPos, Vector3[] vertices, Transform parent)
        {
            Dictionary<Vector3, TerrainObjectInstance> itemsToSpawn = new Dictionary<Vector3, TerrainObjectInstance>();

            foreach(TerrainEnvironmentItem item in perTileItems)
            {
                //set a random variable for the numbre of objects to place
                var upper = Random.Range(item.numToSpawn.x, item.numToSpawn.y);

                int i = upper;
                int j = 100;    //try not more than 100 times
                while(i > 0 && j > 0)
                {
                    var vertex = vertices[Random.Range(0, vertices.Length)];
                    if(!itemsToSpawn.ContainsKey(vertex))
                    {
                        var record = CreateEmptyObjectInstance(vertex + worldPos, parent, item.itemAsset);
                        itemsToSpawn.Add(vertex,record);
                        i--;
                    }
                    j--;
                }
            }

            foreach(KeyValuePair<Vector3, TerrainObjectInstance> item in itemsToSpawn)
            {
                item.Value.SetGO(GetFromPool(item.Value));
            }

            return itemsToSpawn;
        }

        private TerrainObjectInstance CreateEmptyObjectInstance(Vector3 vertexPos, Transform parent, TerrainObjectAsset asset)
        {
            var item = new TerrainObjectInstance();
            item.SetPos(vertexPos);
            item.SetRot(GetRandomObjectRotation(asset,parent.rotation));
            item.SetScale(asset.GetRandomScale());
            item.SetAsset(asset);

            return item;
        }

        public GameObject GetFromPool(TerrainObjectInstance instance)
        {
            var go = objectPool.GetObjectFromPool(instance.Asset(), instance.Pos(), Quaternion.Euler(instance.Rot()));
            go.transform.localScale = new Vector3(
                instance.Scale(),
                instance.Scale(),
                instance.Scale()
                );
            
            return go;
        }

        private Vector3 GetRandomObjectRotation(TerrainObjectAsset asset, Quaternion baseRotation)
        {
            return asset.GetRandomRot(baseRotation.eulerAngles);
        }

        
        #endregion

        #region old stuff
        public List<TerrainEnvironmentItem> GetPerTileEnvironmentItems()
        {
            return perTileItems;
        }

        public List<TerrainEnvironmentItem> GetWorldEnvironmentItems()
        {
            return worldEnvironmentItems;
        }

        public Material GetGroundMat() { return groundMat; }
        #endregion
    }
}


