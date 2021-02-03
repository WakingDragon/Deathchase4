using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dev/New Example Poolable Asset", fileName ="ExampleAsset")]
public class ExampleObjectAsset : ScriptableObject, IPoolableAsset
{
    [SerializeField] private GameObject prefab = null;

    [Header("Pooling settings")]
    [SerializeField] private int defaultNumberToPool = 5;
    [SerializeField] private bool expandablePool = true;
    [SerializeField] private float m_autoRepoolLife = 2f;

    public string GetName()
    {
        return this.name.ToString();
    }
    public IPoolableAsset GetIPoolableAsset() { return this; }
    public int GetNumberToPool() { return defaultNumberToPool; }
    public bool GetIsPoolExpandable() { return expandablePool; }

    public GameObject InstantiatePrefabToPool(Transform parent)
    {
        //This can be used to put in very unique elements to pooled object spawning
        GameObject go = Instantiate(prefab, parent);
        return go;
    }

    public float AutoRepool()
    {
        return m_autoRepoolLife;
    }
}
