using UnityEngine;

namespace Combat.Core
{
    /// <summary>
    /// An interface that represents a thing that can do damage.
    /// </summary>
    ///
    /// <example>
    /// <list type="number">
    ///     <item>A bullet that is shot from a weapon will to damage when hit something.</item>
    ///     <item>Swinging a sword will create an area that does damage to targets within the area.</item>
    /// </list>
    /// </example>
    public interface IDamagingEntity
    {
        /// <summary>
        /// The <see cref="GameObject"/> that this <see cref="IDamagingEntity"/> belongs to.
        /// </summary>
        public GameObject Source { get; }
        
        /// <summary>
        /// The <see cref="Rigidbody"/> of the  <see cref="IDamagingEntity"/>.
        /// </summary>
        public Rigidbody Rb { get; }
        
        /// <summary>
        /// The <see cref="Collider"/> of the  <see cref="IDamagingEntity"/>.
        /// </summary>
        public Collider Collider { get; }

        /// <summary>
        /// Do an amount of <paramref name="amount"/> damage to <paramref name="damageable"/>.
        /// </summary>
        /// <param name="damageable">The target.</param>
        /// <param name="amount">The amount af damage to deal.</param>
        public void DoDamage(IDamageable damageable, float amount);
    }
}