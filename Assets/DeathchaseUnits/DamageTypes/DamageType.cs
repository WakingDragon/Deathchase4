using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="new_dmgType",menuName ="Units/DamageTypes/Damage Type")]
    public class DamageType : ScriptableObject
    {
        [SerializeField] private string m_name = "unnamed damage type";
        [SerializeField] private FXAsset m_defaultDmgFX = null;
        [SerializeField] private DamageTypeLibrary m_library;

        public string Name() { return m_name; }
        public FXAsset DefaultDmgFX() { return m_defaultDmgFX; }

        public void SetDamageTypeLibrary(DamageTypeLibrary library)
        {
            m_library = library;
        }
    }
}

