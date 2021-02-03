using UnityEngine;

namespace BP.ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {
        private ObjectPoolAsset m_poolAsset;

        private void OnDisable()
        {
            if (m_poolAsset) { m_poolAsset.SetIsInitialised(false); }
        }

        public void SetPoolAsset(ObjectPoolAsset asset) { m_poolAsset = asset; }
    }
}
//    {
//        [Header("core assets")]
//        [SerializeField] private GameScenesLibrary sceneLibrary = null;

//        [Header("object pool")]
//        [SerializeField] private List<PoolTypes> poolTypes = new List<PoolTypes>();
//        private Dictionary<string, List<GameObject>> poolMaster = new Dictionary<string, List<GameObject>>();
//        private List<PoolTypes> poolTypesToDelete = new List<PoolTypes>();

//        #region singleton
//        public static ObjectPool instance;

//        private void Awake()
//        {
//            instance = this;
//        }
//        #endregion

//        private void OnEnable()
//        {
//            if (!sceneLibrary) { Debug.Log("need the GameScenesLibrary"); }

//            RegisterForDelegates();
//        }

//        private void OnDisable()
//        {
//            UnregisterForDelegates();
//        }

//        #region managing pools
//        public void TryCreateNewPool(IPoolableAsset newPoolAsset)
//        {
//            if (PoolExistsInMaster(newPoolAsset)) { return; }
//            var newPoolParams = CreatePoolFromAsset(newPoolAsset);
//            AddPoolToMaster(newPoolParams);
//        }

//        //public void TryCreateNewPool(IPoolableAsset newPoolAsset, int numToPool)
//        //{
//        //    if (PoolExistsInMaster(newPoolAsset)) { return; }
//        //    var newPoolParams = CreatePoolFromAsset(newPoolAsset, numToPool);
//        //    AddPoolToMaster(newPoolParams);
//        //}

//        private bool PoolExistsInMaster(IPoolableAsset poolAsset)
//        {
//            var assetName = poolAsset.GetName();

//            if (poolMaster.ContainsKey(assetName)) { return true; }
//            return false;
//        }

//        private PoolTypes CreatePoolFromAsset(IPoolableAsset poolAsset)
//        {
//            PoolTypes newPool = new PoolTypes
//            {
//                tag = poolAsset.GetName(),
//                asset = poolAsset.GetIPoolableAsset(),
//                prefab = null,
//                size = poolAsset.GetNumberToPool(),
//                canExpand = poolAsset.GetIsPoolExpandable(),
//                isSelfInstantiating = true,
//                sceneName = GetActiveScene()
//            };
//            poolTypes.Add(newPool);
//            return newPool;
//        }

//        private PoolTypes CreatePoolFromAsset(IPoolableAsset poolAsset, int numToPool)
//        {
//            PoolTypes newPool = new PoolTypes
//            {
//                tag = poolAsset.GetName(),
//                asset = poolAsset.GetIPoolableAsset(),
//                prefab = null,
//                size = numToPool,
//                canExpand = poolAsset.GetIsPoolExpandable(),
//                isSelfInstantiating = true,
//                sceneName = GetActiveScene()
//            };
//            poolTypes.Add(newPool);
//            return newPool;
//        }
//        private string GetActiveScene()
//        {
//            return SceneManager.GetActiveScene().name;
//        }
//        #endregion

//        #region deleting pools and objects
//        //public void DeleteObjectPoolBySceneName(string sceneName)
//        //{
//        //    poolTypesToDelete.Clear();

//        //    foreach (PoolTypes poolType in poolTypes)
//        //    {
//        //        if (poolType.sceneName == sceneName)
//        //        {
//        //            string tag = poolType.tag;
//        //            DeleteAllObjectsInPool(tag);
//        //            DeleteDictEntryForTag(tag);
//        //            poolTypesToDelete.Add(poolType);
//        //        }
//        //    }

//        //    foreach (PoolTypes poolToDelete in poolTypesToDelete)
//        //    {
//        //        poolTypes.Remove(poolToDelete);
//        //    }

//        //    poolTypesToDelete.Clear();
//        //}

//        //private void DeleteAllObjectsInPool(string tag)
//        //{
//        //    if (poolMaster.ContainsKey(tag))
//        //    {
//        //        var list = poolMaster[tag];
//        //        foreach (GameObject obj in list)
//        //        {
//        //            Destroy(obj);
//        //        }
//        //    }
//        //}

//        //private void DeleteDictEntryForTag(string tag)
//        //{
//        //    if (poolMaster.ContainsKey(tag))
//        //    {
//        //        poolMaster.Remove(tag);
//        //    }
//        //}
//        #endregion

//        #region instantiate the pools to master
//        private void AddPoolToMaster(PoolTypes newPoolRef)
//        {
//            List<GameObject> objectPool = InstantiatePoolObjectsAndAddToMaster(newPoolRef);
//            poolMaster.Add(newPoolRef.tag, objectPool);
//        }

//        private List<GameObject> InstantiatePoolObjectsAndAddToMaster(PoolTypes pool)
//        {
//            List<GameObject> objectPool = new List<GameObject>();

//            for (int i = 0; i < pool.size; i++)
//            {
//                var go = AddNewGameObjectToPool(pool.asset, objectPool);
//                go.SetActive(false);
//            }

//            return objectPool;
//        }

//        private GameObject AddNewGameObjectToPool(IPoolableAsset asset, List<GameObject> objectPool)
//        {
//            GameObject go = asset.InstantiatePrefabToPool(transform);
//            int prefix = objectPool.Count + 1;
//            go.name = prefix + " " + asset.GetName();

//            if (go)
//            {
//                objectPool.Add(go);
//            }
//            return go;
//        }
//        #endregion

//        #region get and return to pool
//        public GameObject GetObjectFromPool(IPoolableAsset asset, Vector3 position, Quaternion rotation)
//        {
//            if (position == null) { position = Vector3.zero; }
//            if (rotation == null) { rotation = Quaternion.identity; }

//            if (!PoolExistsInMaster(asset))
//            {
//                TryCreateNewPool(asset);
//                return null;        //TODO - this means the first GO will always be missing when adding on the fly
//            }

//            var objectPool = poolMaster[asset.GetName()];

//            var go = FindInactiveObjectInPool(objectPool);

//            if (go == null)
//            {
//                go = AddNewGameObjectToPool(asset, objectPool);
//                go.SetActive(true);
//            }

//            if (go != null)
//            {
//                //go.transform.parent = null;
//                SetObjectPositionAndRotation(go, position, rotation);
//            }

//            return go;
//        }

//        public void Repool(GameObject go)
//        {
//            go.transform.parent = null;
//            go.SetActive(false);
//            go.transform.parent = transform;
//            go.transform.position = Vector3.zero;
//            go.transform.rotation = Quaternion.identity;
//        }

//        private GameObject FindInactiveObjectInPool(List<GameObject> poolToUse)
//        {
//            for (int i = 0; i < poolToUse.Count; i++)
//            {
//                if (!poolToUse[i].activeInHierarchy)  //activeInHierarchy)
//                {
//                    GameObject obj = poolToUse[i].gameObject;
//                    obj.SetActive(true);
//                    return obj;
//                }
//            }
//            return null;
//        }

//        private void SetObjectPositionAndRotation(GameObject go, Vector3 position, Quaternion rotation)
//        {
//            go.transform.position = position;
//            go.transform.rotation = rotation;
//        }
//        #endregion

//        private void RegisterForDelegates()
//        {
//            //if (SceneLoader.instance) { SceneLoader.onSceneUnloaded += DeleteObjectPoolBySceneName; }
//        }

//        private void UnregisterForDelegates()
//        {
//            //if (SceneLoader.instance) { SceneLoader.onSceneUnloaded -= DeleteObjectPoolBySceneName; }
//        }

//    }
//}

