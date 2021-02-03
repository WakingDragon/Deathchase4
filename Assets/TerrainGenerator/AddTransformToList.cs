using UnityEngine;
using BP.Core;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class AddTransformToList : MonoBehaviour
    {
        [SerializeField] private Transform_listSet transformList = null;

        private void OnEnable()
        {
            if (!transformList) { Debug.Log("no list asset on " + gameObject.name); }
            transformList.AddToList(transform);
        }

        private void OnDisable()
        {
            transformList.RemoveFromList(transform);
        }
    }
}

