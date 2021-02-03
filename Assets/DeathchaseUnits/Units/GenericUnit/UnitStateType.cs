using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units
{
    public enum UnitStateType
    {
        none,
        assembled,
        idling,
        alive,
        dead
    }

    /// states:
    /// none - the default
    /// assembled - objects created and wired up
    /// idling - non-active meta state
    /// alive - all operational
    /// dead - may still be in scene, prior to returning to assembled state
}

