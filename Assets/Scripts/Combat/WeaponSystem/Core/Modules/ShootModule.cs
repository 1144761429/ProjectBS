using System;
using System.Collections;
using Combat.Core;
using Combat.WeaponSystem.Core.Modules.EventArguments;
using Extensions;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Combat.WeaponSystem.Core.Modules
{
    /// <summary>
    /// A <see cref="WeaponModule"/> that handles functionality of shooting.
    /// </summary>
    ///
    /// <example>
    /// Auto/semi-auto, single/burst shooting mode
    /// </example>
    public sealed class ShootModule : WeaponModule
    {
        #region Events
        
        /// <summary>
        /// An event that will trigger after passing all the <see cref="_shootCondition"/>, and before perform a shoot.
        /// </summary>
        public EventHandler<WeaponShootEventArgs> BeforeShoot { get; set; }
        
        /// <summary>
        /// An event that will trigger when is shooting(right after the <see cref="Projectile"/> is set active and shoot
        /// out).
        /// </summary>
        public EventHandler<WeaponShootEventArgs> OnShoot { get; set; }
        
        /// <summary>
        /// An event that will trigger after <see cref="OnShoot"/>.
        /// </summary>
        public EventHandler<WeaponShootEventArgs> AfterShoot { get; set; }
        
        #endregion

        #region Fields and Properties
        
        /// <inheritdoc/>
        public override EWeaponModuleType ModuleType => EWeaponModuleType.ShootModule;

        /// <summary>
        /// The <see cref="ShootPattern"/> that this <see cref="ShootModule"/> follows. This is provided by the
        /// <see cref="WeaponModule.SourceWeapon"/>.
        /// </summary>
        private ShootPattern _shootPattern;

        /// <summary>
        /// The <see cref="WeaponModule.SourceWeapon"/> casted to <see cref="RangeWeapon"/>.
        /// This will be null if the cast fail. If a cast fails, an <see cref="IncompatibleModuleException"/> will
        /// be thrown.
        /// </summary>
        private RangeWeapon _sourceRangeWeapon;

        /// <summary>
        /// The condition for if this <see cref="ShootModule"/> can shoot.
        /// </summary>
        private Func<bool> _shootCondition;

        #endregion
        
        #region Public Function

        /// <summary>
        /// Create a <see cref="ShootModule"/> that is attached to <see cref="WeaponModule.SourceWeapon"/>.
        /// </summary>
        /// 
        /// <param name="sourceWeapon">The <see cref="Weapon"/> that this <see cref="WeaponModule"/> is attached on.</param>
        /// <param name="shootPattern">The <see cref="ShootPattern"/> that this <see cref="ShootModule"/> follows.</param>
        /// 
        /// <exception cref="IncompatibleModuleException">If the <paramref name="sourceWeapon"/> is not compatible with
        /// this module.</exception>
        public ShootModule(Weapon sourceWeapon, ShootPattern shootPattern) : base(sourceWeapon)
        {
            if (SourceWeapon is not RangeWeapon)
            {
                string message = $"{typeof(ShootModule)} is only compatible to {typeof(RangeWeapon)}.";
                throw new IncompatibleModuleException(sourceWeapon.DatabaseID, typeof(AmmunitionModule), message);
            }

            _sourceRangeWeapon = (RangeWeapon)SourceWeapon;
            _shootPattern = shootPattern;
            
            //TODO: change the shoot condition according to the fire mode of the weapon.
        }
        
        /// <summary>
        /// Shoot the <see cref="_sourceRangeWeapon"/>.
        /// </summary>
        /// 
        /// <param name="direction">The direction to shoot.</param>
        ///
        /// <returns><c>true</c> if shoot is successfully performed. Otherwise, <c>false</c>.</returns>
        public bool Shoot(Vector3 direction)
        {
            if (!_shootCondition.AllTrue())
            {
                return false;
            }
            
            _sourceRangeWeapon.StartCoroutine(ProcessShootPattern(direction));

            return true;
        }
        
        #endregion
        
        #region Private Functions
        
        /// <summary>
        /// Shoot at direction <paramref name="direction"/> and apply direction modification described in
        /// the <see cref="ShootPattern"/> of the <see cref="Weapon"/> this <see cref="ShootModule"/> attached to.
        /// And trigger events of <see cref="BeforeShoot"/>, <see cref="OnShoot"/>, and <see cref="AfterShoot"/> accordingly.
        /// </summary>
        /// 
        /// <param name="direction">The direction to shoot. This is a general direction. The actual shoot direction will
        /// be affected by the direction and offset described in <see cref="ShootPattern"/>. </param>
        private IEnumerator ProcessShootPattern(Vector3 direction)
        {
            WeaponShootEventArgs eventArgs =
                new WeaponShootEventArgs(_sourceRangeWeapon.DatabaseID, _sourceRangeWeapon.Wielder);
            
            BeforeShoot?.Invoke(this, eventArgs);
            
            // Retrieve all the projectile prefab that is needed.
            _sourceRangeWeapon.AmmunitionModule.GetProjectile(_shootPattern.ProjectileCount, out Projectile[] projectiles);
            
            // An index that tracks which projectile we are currently using in the Projectile[] projectiles
            int currentProjectileIndex = 0;
            
            for (int roundIndex = 0; roundIndex < _shootPattern.RoundCount; roundIndex++)
            {
                ShootPatternRound round = _shootPattern.Rounds[roundIndex];
                
                for (int projectileIndex = 0; projectileIndex < round.ProjectileCount; projectileIndex++)
                {
                    SingleProjectilePattern singleProjectilePattern = round.Projectiles[projectileIndex];

                    Projectile projectile = projectiles[currentProjectileIndex];


                    // The direction of the round based on the direction to shoot.
                    Vector3 roundDirection = new Vector3(round.GeneralDirection.x + round.DirectionOffset.x,
                        round.GeneralDirection.y + round.DirectionOffset.y, round.GeneralDirection.z);
                    
                    // The direction of the projectile based on the direction of the round.
                    Vector3 projectileDirection = new Vector3(
                        singleProjectilePattern.GeneralDirection.x + singleProjectilePattern.DirectionOffset.x,
                        singleProjectilePattern.GeneralDirection.y + singleProjectilePattern.DirectionOffset.y,
                        singleProjectilePattern.GeneralDirection.z);

                    projectile.Rb.velocity = (direction + roundDirection + projectileDirection).normalized *
                                             singleProjectilePattern.Speed;

                    currentProjectileIndex++;
                    yield return new WaitForSeconds(singleProjectilePattern.IntervalBeforeNext);
                }

                yield return new WaitForSeconds(round.IntervalBeforeNext);
            }
            
            OnShoot?.Invoke(this, eventArgs);
            AfterShoot.Invoke(this,eventArgs);
        }

        #endregion
    }
}