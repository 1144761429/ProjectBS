using System;
using UnityEngine;

namespace Combat.Core
{
    /// <summary>
    /// An <see cref="EventArgs"/> that is used when a <see cref="IDamageable"/> object receives damage, including
    /// 0 damage.
    /// </summary>
    public class OnTakingDamageEventArgs : EventArgs
    {
        /// <summary>
        /// Where the damage come from.
        /// </summary>
        public readonly GameObject Source;
        
        /// <summary>
        /// The <see cref="IDamagingEntity"/> that deals the damage.
        /// </summary>
        public readonly IDamagingEntity DamagingEntity;

        /// <summary>
        /// The <see cref="IDamageable"/> object that takes the damage.
        /// </summary>
        public readonly IDamageable Receiver;
        
        /// <summary>
        /// The actual amount of damage <see cref="Receiver"/> takes. 
        /// </summary>
        public readonly float DamageReceived;
        
        //TODO: add another field of DamageBeforeReduction?
        
        /// <summary>
        /// A constructor that creates an <see cref="OnTakingDamageEventArgs"/> with certain <see cref="DamagingEntity"/>,
        /// <see cref="Receiver"/>, and <see cref="DamageReceived"/>. 
        /// </summary>
        /// <param name="damagingEntity">The <see cref="IDamagingEntity"/> that deals the damage.</param>
        /// <param name="receiver">The <see cref="IDamageable"/> object that takes the damage.</param>
        /// <param name="damageReceived">The actual amount of damage <see cref="Receiver"/> takes. </param>
        public OnTakingDamageEventArgs(IDamagingEntity damagingEntity, IDamageable receiver, float damageReceived)
        {
            Source = damagingEntity.Source;
            DamagingEntity = damagingEntity;
            Receiver = receiver;
            DamageReceived = damageReceived;
        }
    }
}