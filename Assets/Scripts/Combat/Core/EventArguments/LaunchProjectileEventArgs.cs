using System;

namespace Combat.Core.EventArguments
{
    /// <summary>
    /// An <see cref="EventArgs"/> that is used when a <see cref="IProjectileLauncher"/>
    /// <see cref="IProjectileLauncher.Launch"/> a <see cref="Projectile"/>.
    /// </summary>
    public class LaunchProjectileEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IProjectileLauncher"/> that launches the <see cref="Projectile"/>.
        /// </summary>
        public readonly IProjectileLauncher Launcher;
        
        /// <summary>
        /// The <see cref="Projectile"/> being launched.
        /// </summary>
        public readonly Projectile Projectile;
        
        /// <summary>
        /// A constructor that creates an <see cref="LaunchProjectileEventArgs"/> with certain <see cref="Launcher"/>
        /// and <see cref="Projectile"/>.
        /// </summary>
        /// 
        /// <param name="launcher">The <see cref="IProjectileLauncher"/> that launches the <see cref="Projectile"/>.</param>
        /// <param name="projectile">The <see cref="Projectile"/> being launched.</param>
        public LaunchProjectileEventArgs(IProjectileLauncher launcher, Projectile projectile)
        {
            Launcher = launcher;
            Projectile = projectile;
        }
    }
}