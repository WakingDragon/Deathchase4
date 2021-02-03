using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public class GameEventTestRaiser : MonoBehaviour
    {
        [SerializeField] private VoidGameEvent on1Pressed = null;
        [SerializeField] private IntGameEvent onIntSupplied = null;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                on1Pressed.Raise();
                onIntSupplied.Raise(42);
            }

        }
    }
}

