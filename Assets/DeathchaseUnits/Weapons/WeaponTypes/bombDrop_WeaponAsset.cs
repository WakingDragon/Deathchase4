using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="new_bombWeapon",menuName ="Units/Weapons/Bomb Weapon")]
    public class bombDrop_WeaponAsset : WeaponAsset
    {
        public override void Fire(Transform launchPoint, Faction faction)
        {
            base.Fire(launchPoint, faction);
        }
    }
}

