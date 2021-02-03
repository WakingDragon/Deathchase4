using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BP.Core;

namespace BP.Deathchase
{
    public class UnitNotifyCam : MonoBehaviour
    {
        [SerializeField] private TransformGameEvent playerOnPitchGameEvent = null;

        private void Start()
        {
            playerOnPitchGameEvent.Raise(transform);
        }
    }
}

