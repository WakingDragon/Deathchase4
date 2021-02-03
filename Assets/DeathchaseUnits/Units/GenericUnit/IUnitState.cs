using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units
{
    public interface IUnitState
    {
        void OnEnterNewUnitState(UnitStateType newState);
    }
}

