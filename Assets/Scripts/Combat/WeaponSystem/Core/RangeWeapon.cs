using Combat.Core;
using Combat.WeaponSystem.Core.Modules;
using UnityEngine;

namespace Combat.WeaponSystem.Core
{
    public class RangeWeapon : Weapon
    {
        [field: SerializeField] public Projectile Projectile { get; private set; }
        
        public AmmunitionModule AmmunitionModule { get; private set; }
        public ShootModule ShootModule { get; private set; }
    }
}