using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Units.Weapons
{
    [CreateAssetMenu(fileName ="new_ammoAsset",menuName ="Units/Ammo/Bullet Ammo Asset")]
    public class bullet_AmmoAsset : WeaponAmmoAsset
    {
        protected override void AssembleAmmo(GameObject go)
        {
            go.AddComponent<bullet_weaponAmmo>().SetupAmmo(this);
        }
    }
}

