using System;

namespace Combat.WeaponSystem
{
    /// <summary>
    /// <para>
    /// An abstract class that represents a module of a weapon.
    /// </para>
    ///
    /// <para>
    /// A <see cref="Weapon"/> is made by different modules, or functionalities. By adding different
    /// <see cref="WeaponModule"/>, a <see cref="Weapon"/> achieves its completed functionalities.
    /// </para>
    /// </summary>
    public abstract class WeaponModule
    {
        /// <summary>
        /// The <see cref="Enum"/> that represents the type of this <see cref="WeaponModule"/>.
        /// </summary>
        public abstract EWeaponModuleType ModuleType { get; }

        /// <summary>
        /// The <see cref="Weapon"/> that this <see cref="WeaponModule"/> is attached on.
        /// </summary>
        public abstract Weapon SourceWeapon { get; }
        
        
    }
}