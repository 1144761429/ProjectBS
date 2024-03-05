using System;
using UnityEngine;

namespace Combat.Core
{
    /// <summary>
    /// An <see cref="EventArgs"/> that is used when a <see cref="IDamageable"/> object hit something.
    /// </summary>
    public class OnDamagingEntityHitEventArgs : EventArgs
    {
        /// <summary>
        /// Where the damage come from.
        /// </summary>
        public readonly GameObject Source;
        
        /// <summary>
        /// The <see cref="IDamagingEntity"/> that hits something.
        /// </summary>
        public readonly IDamagingEntity DamagingEntity;

        /// <summary>
        /// The <see cref="Collider"/> that the <see cref="DamagingEntity"/> hits.
        /// </summary>
        public readonly Collider Collidee;
        
        /// <summary>
        /// A constructor that creates an <see cref="OnDamagingEntityHitEventArgs"/> with certain <see cref="DamagingEntity"/>,
        /// and <see cref="Collidee"/>.
        /// </summary>
        /// <param name="damagingEntity">The <see cref="IDamagingEntity"/> that hits something.</param>
        /// <param name="collidee">The <see cref="Collider"/> that the <see cref="DamagingEntity"/> hits.</param>
        public OnDamagingEntityHitEventArgs(IDamagingEntity damagingEntity, Collider collidee)
        {
            Source = damagingEntity.Source;
            DamagingEntity = damagingEntity;
            Collidee = collidee;
        }
    }
}