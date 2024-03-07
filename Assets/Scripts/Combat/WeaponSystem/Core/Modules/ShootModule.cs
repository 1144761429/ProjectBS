namespace Combat.WeaponSystem.Core.Modules
{
    /// <summary>
    /// A <see cref="WeaponModule"/> that handles functionality of shooting.
    /// </summary>
    public sealed class ShootModule : WeaponModule
    {
        /// <inheritdoc/>
        public override EWeaponModuleType ModuleType => EWeaponModuleType.ShootModule;
        
        /// <inheritdoc/>
        public ShootModule(Weapon sourceWeapon) : base(sourceWeapon)
        {
        }
    }
}