using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="new_missileWeapon",menuName ="Units/Weapons/Missile Launcher")]
    public class missile_WeaponAsset : WeaponAsset
    {
        public override void Fire(Transform launchPoint, Faction faction)
        {
            base.Fire(launchPoint, faction);
        }
    }
}

