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
        public EventHandler<WeaponShootEventArgs> BeforeShoot { get; set; }
        public EventHandler<WeaponShootEventArgs> OnShoot { get; set; }
        public EventHandler<WeaponShootEventArgs> AfterShoot { get; set; }
        
        #endregion

        #region Fields and Properties

        public override EWeaponModuleType ModuleType => EWeaponModuleType.ShootModule;

        private ShootPattern _shootPattern;

        private RangeWeapon _sourceRangeWeapon;

        private Func<bool> _shootCondition;
        
        #endregion
        
        #region Public Function

        /// <inheritdoc/>
        public ShootModule(Weapon sourceWeapon, ShootPattern shootPattern) : base(sourceWeapon)
        {
            if (SourceWeapon is not RangeWeapon)
            {
                string message = $"{typeof(ShootModule)} is only compatible to {typeof(RangeWeapon)}.";
                throw new IncompatibleModuleException(sourceWeapon.DatabaseID, typeof(AmmunitionModule), message);
            }

            _sourceRangeWeapon = (RangeWeapon)SourceWeapon;
            _shootPattern = shootPattern;
        }
        
        /// <summary>
        /// Shoot the <see cref="_sourceRangeWeapon"/>and trigger events accordingly.
        /// </summary>
        /// 
        /// <param name="direction">The direction to shoot.</param>
        /// 
        /// <returns><c>true</c> if shoot successfully. Otherwise, meaning the <see cref="_shootCondition"/> is not
        /// satisfied, return <c>false</c>.</returns>
        public bool Shoot(Vector3 direction)
        {
            if (!_shootCondition.AllTrue())
            {
                return false;
            }
            
            WeaponShootEventArgs eventArgs =
                new WeaponShootEventArgs(_sourceRangeWeapon.DatabaseID, _sourceRangeWeapon.Wielder);
            
            BeforeShoot?.Invoke(this, eventArgs);
            _sourceRangeWeapon.StartCoroutine(ProcessShootPattern(direction));
            
            // TODO: Make the function continue the execution until the coroutine above is finished
            OnShoot?.Invoke(this, eventArgs);
            AfterShoot.Invoke(this,eventArgs);
            
            return true;
        }

        #endregion
        
        #region Private Functions

        /// <summary>
        /// Shoot at direction <paramref name="direction"/> and apply direction modification described in
        /// the <see cref="ShootPattern"/> of the <see cref="Weapon"/> this <see cref="ShootModule"/> attached to.
        /// </summary>
        /// 
        /// <param name="direction">The direction to shoot. This is a general direction. The actual shoot direction will
        /// be affected by the direction and offset described in <see cref="ShootPattern"/>. </param>
        private IEnumerator ProcessShootPattern(Vector3 direction)
        {
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
        }

        #endregion
    }
}