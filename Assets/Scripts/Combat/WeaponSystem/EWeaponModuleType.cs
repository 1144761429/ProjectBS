namespace Combat.WeaponSystem
{
    /// <summary>
    /// An enum for all possible <see cref="WeaponModule"/>s.
    /// </summary>
    public enum EWeaponModuleType
    {
        // Base Modules for range weapon.
        ShootModule = 0,
        AmmunitionModule = 1,
        AimModule = 2,
        
        // Base Module for melee weapon.
        MeleeAttackModule = 3,
    }
}