using UnityEngine;

namespace BP.Core
{
    [CreateAssetMenu(fileName ="void_gameEvent",menuName ="Core/Game Events/Void Game Event")]
    public class VoidGameEvent : BaseGameEvent<VoidType>
    {
        public void Raise()
        {
            Raise(new VoidType());
        }
    }
}


