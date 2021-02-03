using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}
