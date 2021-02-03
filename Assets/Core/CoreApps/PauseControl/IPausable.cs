using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public interface IPausable
    {
        void OnPause(bool pause);
    }
}

