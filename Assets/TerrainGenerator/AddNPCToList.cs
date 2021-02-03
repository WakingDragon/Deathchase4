using UnityEngine;
using BP.Core;

namespace BP.Worlds.ProceduralTerrainGenerator
{
    public class AddNPCToList : MonoBehaviour
    {
        [SerializeField] private Transform_listSet list = null;

        private void OnEnable()
        {
            list.AddToList(transform);
        }

        private void OnDisable()
        {
            list.RemoveFromList(transform);
        }
    }
}

