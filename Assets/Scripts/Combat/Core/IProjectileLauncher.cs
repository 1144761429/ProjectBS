using System;
using Combat.Core.EventArguments;

namespace Combat.Core
{
    /// <summary>
    /// An interface that defines what a projectile launcher should have.
    /// </summary>
    public interface IProjectileLauncher
    {
        /// <summary>
        /// A <see cref="Func{TResult}"/> that stores the conditions need to met before launch.
        /// </summary>
        public event Func<bool> LaunchCondition;

        /// <summary>
        /// An event that is triggered after <see cref="LaunchCondition"/> is passed and before launch.
        /// </summary>
        public EventHandler<LaunchProjectileEventArgs> BeforeLaunch { get; }
        
        /// <summary>
        /// An event that is triggered after <see cref="BeforeLaunch"/> and before <see cref="AfterLaunch"/>.
        /// </summary>
        public EventHandler<LaunchProjectileEventArgs> OnLaunch { get; }
        
        /// <summary>
        /// An event that is triggered after <see cref="OnLaunch"/> and before launch.
        /// </summary>
        public EventHandler<LaunchProjectileEventArgs> AfterLaunch { get; }
        
        /// <summary>
        /// The <see cref="ShootPattern"/> that this <see cref="IProjectileLauncher"/> follows.
        /// </summary>
        public ShootPattern ShootPattern { get; }
        
        /// <summary>
        /// Launch this <see cref="IProjectileLauncher"/> according to <see cref="ShootPattern"/>.
        /// </summary>
        public void Launch();
    }
}