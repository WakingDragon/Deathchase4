using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableAsset
{
    GameObject InstantiatePrefabToPool(Transform parent);
    string GetName();
    IPoolableAsset GetIPoolableAsset();
    int GetNumberToPool();
    bool GetIsPoolExpandable();
    float AutoRepool();
}
