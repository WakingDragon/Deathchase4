using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="damageTypeLibraryAsset",menuName ="Units/DamageTypes/(single) Damage Type Library")]
    public class DamageTypeLibrary : ScriptableObject
    {
        [SerializeField] private List<DamageType> items = new List<DamageType>();

        public List<DamageType> GetList() { return items; }

        public DamageType GetItemIndex(int index)
        {
            return items[0];
        }

        public void AddToList(DamageType item)
        {
            if(!items.Contains(item)) { items.Add(item); }
        }

        public void RemoveFromList(DamageType item)
        {
            if (items.Contains(item)) { items.Remove(item); }
        }

        public int Count() { return items.Count; }

        private void OnEnable()
        {
            foreach(DamageType item in items)
            {
                item.SetDamageTypeLibrary(this);
            }
        }
    }
}

