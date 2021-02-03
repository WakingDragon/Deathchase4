using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Deathchase
{
    [CreateAssetMenu(fileName ="DCGameStateLibrary",menuName ="Deathchase/(single) Game State Library")]
    public class DCGameStateLibrary : ScriptableObject
    {
        [SerializeField] private DCGameStateAsset[] states;

        public void EnterState(DCGameStateAsset state)
        {
            //do some actions
        }
    }
}

