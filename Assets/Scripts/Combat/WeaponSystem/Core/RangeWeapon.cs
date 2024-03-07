using Combat.Core;
using UnityEngine;

namespace Combat.WeaponSystem.Core
{
    public class RangeWeapon : Weapon
    {
        [field: SerializeField] public Projectile Projectile { get; private set; }
    }
}