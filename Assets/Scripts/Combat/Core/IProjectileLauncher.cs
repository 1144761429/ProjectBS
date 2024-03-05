namespace Combat.Core
{
    /// <summary>
    /// An interface that defines what a projectile launcher should have.
    /// </summary>
    public interface IProjectileLauncher
    {
        //TODO:  add events.
        
        /// <summary>
        /// Launch this <see cref="IProjectileLauncher"/> according to <paramref name="projectilePattern"/>.
        /// </summary>
        /// 
        /// <param name="projectilePattern">The <see cref="ProjectilePattern"/> that will follow when launch.</param>
        public void Launch(ProjectilePattern projectilePattern);
    }
}