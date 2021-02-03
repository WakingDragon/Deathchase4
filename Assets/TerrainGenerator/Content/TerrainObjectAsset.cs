using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Terrain Generator/Terrain Object Asset", fileName ="TerrainObjectAsset")]
public class TerrainObjectAsset : ScriptableObject, IPoolableAsset
{
    [Header("Pooling settings")]
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private int defaultNumberToPool = 5;
    [SerializeField] private bool expandablePool = true;
    [SerializeField] private float m_autoRepoolLife = 0f;

    [Header("game settings")]
    [SerializeField] private bool isAboveSeaLevel = false;

    [Header("variety")]
    [SerializeField] private bool rndScale = false;
    [Range(1.1f,10f)][SerializeField] private float maxScale = 1f;
    [SerializeField] private bool rndRot = false;

    #region game-specific
    public bool IsAboveSeaLevel() { return isAboveSeaLevel; }

    public Vector3 GetRandomRot(Vector3 startRotation)
    {
        float angle = 0f;

        if(rndRot)
        {
            angle = Random.Range(0f, 359f);
        }
        
        return new Vector3(
            startRotation.x,
            angle + startRotation.y,
            startRotation.z
            );
    }

    public float GetRandomScale()
    {
        if(rndScale)
        {
            return Random.Range(1f, maxScale);
        }
        return 1f;
    }
    
    //public void RandomiseRot(GameObject go)
    //{
    //    if(go)
    //    {
    //        var angle = Random.Range(0f, 359f);
    //        go.transform.eulerAngles = new Vector3(
    //            go.transform.eulerAngles.x,
    //            angle,
    //            go.transform.eulerAngles.z
    //            );
    //    }
    //}

    private GameObject RandomisedGOScale(GameObject go)
    {
        if(rndScale)
        {
            var scale = Random.Range(1f, maxScale);
            Vector3 newSize = go.transform.localScale * scale;
            go.transform.localScale = newSize;
        }

        return go;
    }
    #endregion

    public string GetName()
    {
        return this.name.ToString();
    }
    public IPoolableAsset GetIPoolableAsset() { return this; }
    public int GetNumberToPool() { return defaultNumberToPool; }
    public void SetNumberToPool(int numToPool) { defaultNumberToPool = numToPool; }
    public bool GetIsPoolExpandable() { return expandablePool; }
    public GameObject GetPrefab() { return prefab; }
    public GameObject InstantiatePrefabToPool(Transform parent)
    {
        //This can be used to put in very unique elements to pooled object spawning
        GameObject go = Instantiate(prefab, parent);
        return RandomisedGOScale(go);
    }
    public float AutoRepool()
    {
        return m_autoRepoolLife;
    }
}
