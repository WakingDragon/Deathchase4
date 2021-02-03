using UnityEngine;
using BP.Core;

namespace BP.Units
{
    [CreateAssetMenu(fileName ="new_faction",menuName ="Units/Factions/New Faction")]
    public class Faction : ScriptableObject
    {
        [SerializeField] private FactionLibraryAsset m_library = null;

        public void SetLibrary(FactionLibraryAsset library) { m_library = library; }
        public FactionLibraryAsset Library() { return m_library; }
        public void AddMeToLibrary(Transform t) { m_library.AddTransform(t, this); }
        public void RemoveMeFromLibrary(Transform t)
        { 
            m_library.RemoveTransform(t, this); 
        }
    }
}

