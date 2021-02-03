using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.ObjectPooling
{
    [System.Serializable]
    public class PoolTypes
    {
        public string tag;
        public IPoolableAsset asset;
        public GameObject prefab;
        public int size;
        public bool canExpand;
        public bool isSelfInstantiating;       //tag pools that self-Instantiate via the asset
        public string sceneName;
    }

    [CreateAssetMenu(fileName ="objectPoolAsset",menuName ="ObjectPooling/(single) Object Pool")]
    public class ObjectPoolAsset : ScriptableObject
    {
        [Header("core assets")]
        [SerializeField] private ScenesLibrary m_sceneLibrary = null;

        [Header("object pool")]
        private List<PoolTypes> m_poolTypes = new List<PoolTypes>();
        private Dictionary<string, List<GameObject>> m_poolMaster = new Dictionary<string, List<GameObject>>();
        private List<PoolTypes> m_poolTypesToDelete = new List<PoolTypes>();
        private GameObject m_poolFolder;
        [SerializeField] private bool m_isInitialised;

        private void OnEnable()
        {
            if (!m_sceneLibrary) { Debug.Log("need the GameScenesLibrary"); }
        }

        public void SetIsInitialised(bool isInitialised) { m_isInitialised = isInitialised; }

        private void CheckIfPoolAssetInitialised()
        {
            if(m_isInitialised) { return; }
            m_poolFolder = new GameObject();
            m_poolFolder.transform.position = Vector3.zero;
            m_poolFolder.transform.rotation = Quaternion.identity;
            var pool = m_poolFolder.AddComponent<ObjectPool>();
            pool.SetPoolAsset(this);
            m_poolFolder.name = "ObjectPoolFolder";

            m_isInitialised = true;
        }

        #region creating a new pool
        public bool PoolExistsFor(IPoolableAsset asset)
        {
            for(int i = 0; i < m_poolTypes.Count; i++)
            {
                if(m_poolTypes[i].asset == asset)
                { return true; }
            }
            return false;
        }

        public void TryCreateNewPool(IPoolableAsset newPoolAsset)
        {
            CheckIfPoolAssetInitialised();

            var assetName = newPoolAsset.GetName();

            if (m_poolMaster.ContainsKey(assetName)) { return; }

            var newPoolParams = CreatePoolFromAsset(newPoolAsset);
            AddPoolToMaster(newPoolParams);
        }

        private PoolTypes CreatePoolFromAsset(IPoolableAsset poolAsset)
        {
            PoolTypes newPool = new PoolTypes
            {
                tag = poolAsset.GetName(),
                asset = poolAsset.GetIPoolableAsset(),
                prefab = null,
                size = poolAsset.GetNumberToPool(),
                canExpand = poolAsset.GetIsPoolExpandable(),
                isSelfInstantiating = true,
                sceneName = GetActiveScene()
            };
            m_poolTypes.Add(newPool);
            return newPool;
        }

        private string GetActiveScene()
        {
            return m_sceneLibrary.GetActiveScene();
        }
        #endregion

        #region deleting pools and objects
        public void DeleteObjectPoolBySceneName(string sceneName)
        {
            m_poolTypesToDelete.Clear();

            foreach (PoolTypes poolType in m_poolTypes)
            {
                if (poolType.sceneName == sceneName)
                {
                    string tag = poolType.tag;
                    DeleteAllObjectsInPool(tag);
                    DeleteDictEntryForTag(tag);
                    m_poolTypesToDelete.Add(poolType);
                }
            }

            foreach (PoolTypes poolToDelete in m_poolTypesToDelete)
            {
                m_poolTypes.Remove(poolToDelete);
            }

            m_poolTypesToDelete.Clear();
        }

        private void DeleteAllObjectsInPool(string tag)
        {
            if (m_poolMaster.ContainsKey(tag))
            {
                var list = m_poolMaster[tag];
                foreach (GameObject obj in list)
                {
                    Destroy(obj);
                }
            }
        }

        private void DeleteDictEntryForTag(string tag)
        {
            if (m_poolMaster.ContainsKey(tag))
            {
                m_poolMaster.Remove(tag);
            }
        }
        #endregion

        #region add new pool to master and create GOs
        private void AddPoolToMaster(PoolTypes newPoolRef)
        {
            List<GameObject> objectPool = InstantiatePoolObjectsAndAddToMaster(newPoolRef);
            m_poolMaster.Add(newPoolRef.tag, objectPool);
        }

        private List<GameObject> InstantiatePoolObjectsAndAddToMaster(PoolTypes pool)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                var go = AddNewGameObjectToPool(pool.asset, objectPool);
                go.SetActive(false);
            }

            return objectPool;
        }

        private GameObject AddNewGameObjectToPool(IPoolableAsset asset, List<GameObject> objectPool)
        {
            GameObject go = asset.InstantiatePrefabToPool(m_poolFolder.transform);
            ObjectRepool repooler = go.AddComponent<ObjectRepool>();
            repooler.Initialise(asset, this);

            int prefix = objectPool.Count + 1;
            go.name = prefix + " " + asset.GetName();

            if (go)
            {
                objectPool.Add(go);
            }
            return go;
        }
        #endregion

        #region get and return to pool
        public GameObject GetObjectFromPool(IPoolableAsset asset, Vector3 position, Quaternion rotation)
        {
            if (position == null) { position = Vector3.zero; }
            if (rotation == null) { rotation = Quaternion.identity; }

            if (!m_poolMaster.ContainsKey(asset.GetName()))
            {
                TryCreateNewPool(asset);
                //return null;        //TODO - this means the first GO will always be missing when adding on the fly
            }

            var objectPool = m_poolMaster[asset.GetName()];

            var go = FindInactiveObjectInPool(objectPool);

            if (go == null)
            {
                go = AddNewGameObjectToPool(asset, objectPool);
                go.SetActive(true);
            }

            if (go != null)
            {
                //go.transform.parent = null;
                SetObjectPositionAndRotation(go, position, rotation);
            }

            ObjectRepool repoolComponent = go.GetComponent<ObjectRepool>();
            repoolComponent.Activate();

            go.SetActive(true);

            return go;
        }

        public void Repool(GameObject go)
        {
            go.transform.parent = null;
            go.SetActive(false);
            go.transform.parent = m_poolFolder.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }

        private GameObject FindInactiveObjectInPool(List<GameObject> poolToUse)
        {
            for (int i = 0; i < poolToUse.Count; i++)
            {
                if (poolToUse[i] && !poolToUse[i].activeInHierarchy)  //activeInHierarchy)
                {
                    GameObject obj = poolToUse[i].gameObject;
                    obj.SetActive(true);
                    return obj;
                }
            }
            return null;
        }

        private void SetObjectPositionAndRotation(GameObject go, Vector3 position, Quaternion rotation)
        {
            go.transform.position = position;
            go.transform.rotation = rotation;
        }
        #endregion
    }
}

