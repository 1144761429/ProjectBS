using Combat.WeaponSystem.Core;

namespace Combat.WeaponSystem.Modules
{
    /// <summary>
    /// A <see cref="WeaponModule"/> that handles functionality of shooting.
    /// </summary>
    public sealed class ShootModule : WeaponModule
    {
        /// <inheritdoc/>
        public override EWeaponModuleType ModuleType => EWeaponModuleType.ShootModule;

        /// <inheritdoc/>
        public override Weapon SourceWeapon { get; protected set; }
    }
}