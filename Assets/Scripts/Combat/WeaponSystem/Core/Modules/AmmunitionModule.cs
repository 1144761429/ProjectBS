using System;
using BehaviorDesigner.Runtime;
using Combat.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Combat.WeaponSystem.Core.Modules
{
    /// <summary>
    /// A <see cref="WeaponModule"/> that handles functionality related to ammunition.
    /// </summary>
    ///
    /// <example>
    /// Add ammo, reduce ammo, reload.
    /// </example>
    public class AmmunitionModule : WeaponModule
    {
        #region Events

        /// <summary>
        /// <para>
        /// An event that will trigger if successfully reload.
        /// </para>
        ///
        /// <para>
        /// The first generic <c>int</c> is the amount before change, the second one is the amount after change. 
        /// </para>
        /// </summary>
        public event Action<int, int> OnReload; 
        
        /// <summary>
        /// <para>
        /// An event that is triggered when <see cref="MagCurrentAmount"/> changes.
        /// </para>
        ///
        /// <para>
        /// If the <see cref="MagCurrentAmount"/> changes to an invalid number, then this event will not trigger.
        /// </para>
        ///
        /// <para>
        /// The first generic <c>int</c> is the amount before change, the second one is the amount after change. 
        /// </para>
        ///
        /// <remarks>
        /// This event is independent from <see cref="OnReload"/>, meaning if <see cref="OnReload"/> is triggered, this
        /// event will trigger as well. But if this event triggers,, <see cref="OnReload"/> may not be triggered.
        /// </remarks>
        /// </summary>
        public event Action<int, int> OnMagCurrentAmountChange;

        /// <summary>
        /// <para>
        /// An event that is triggered when <see cref="ReserveCurrentAmount"/> changes.
        /// </para>
        /// 
        /// <para>
        /// If the <see cref="ReserveCurrentAmount"/> changes to an invalid number, then this event will not trigger.
        /// </para>
        ///
        /// <para>
        /// The first generic <c>int</c> is the amount before change, the second one is the amount after change. 
        /// </para>
        /// </summary>
        public event Action<int, int> OnReserveCurrentAmountChange;

        /// <summary>
        /// <para>
        /// An event that is triggered when <see cref="MagSize"/> changes.
        /// </para>
        /// 
        /// <para>
        /// If the <see cref="MagSize"/> changes to an invalid number, then this event will not trigger.
        /// </para>
        ///
        /// <para>
        /// The first generic <c>int</c> is the amount before change, the second one is the amount after change. 
        /// </para>
        /// </summary>
        public event Action<int, int> OnMagSizeChange;

        /// <summary>
        /// <para>
        /// An event that is triggered when <see cref="ReserveSize"/> changes.
        /// </para>
        /// 
        /// <para>
        /// If the <see cref="ReserveSize"/> changes to an invalid number, then this event will not trigger.
        /// </para>
        ///
        /// <para>
        /// The first generic <c>int</c> is the amount before change, the second one is the amount after change. 
        /// </para>
        /// </summary>
        public event Action<int, int> OnReserveSizeChange;
        
        #endregion
        
        /// <inheritdoc/>
        public override EWeaponModuleType ModuleType => EWeaponModuleType.AmmunitionModule;

        /// <summary>
        /// The amount of <see cref="Projectile"/>s that can fit into a magazine.
        /// </summary>
        public int MagSize { get; private set; }
        
        /// <summary>
        /// The amount of <see cref="Projectile"/>s that reserve can hold..
        /// </summary>
        public int ReserveSize { get; private set; }
        
        /// <summary>
        /// The current amount of <see cref="Projectile"/>s in the magazine.
        /// </summary>
        public int MagCurrentAmount { get; private set; }
        
        /// <summary>
        /// The current amount of <see cref="Projectile"/>s in the reserve.
        /// </summary>
        public int ReserveCurrentAmount { get; private set; }

        /// <summary>
        /// An <see cref="ObjectPool"/> that stores all the <see cref="Projectile"/> needed by this
        /// <see cref="AmmunitionModule"/>.
        /// </summary>
        private readonly ObjectPool<Projectile> _projectilePool;

        #region Public Functions

        //TODO: add MagSize and ReverseSize as parameter to ctor.
        //TODO: when init object pool, set the default cap according to weapon's mag size. as well as maxSize.
        /// <summary>
        /// Create a <see cref="AmmunitionModule"/> that is attached to <see cref="WeaponModule.SourceWeapon"/>.
        /// </summary>
        /// 
        /// <param name="sourceWeapon">The <see cref="Weapon"/> that this <see cref="WeaponModule"/> is attached on.</param>
        /// 
        /// <exception cref="IncompatibleModuleException">If the <paramref name="sourceWeapon"/> is not compatible with this module.</exception>
        public AmmunitionModule(Weapon sourceWeapon) : base(sourceWeapon)
        {
            if (SourceWeapon is not RangeWeapon)
            {
                string message = $"{typeof(AmmunitionModule)} is only compatible to {typeof(RangeWeapon)}.";
                throw new IncompatibleModuleException(sourceWeapon.DatabaseID, typeof(AmmunitionModule), message);
            }
            
            _projectilePool = new ObjectPool<Projectile>(OnCreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, true, 30,
                120);
        }
        
        /// <summary>
        /// Get a certain amount of projectiles prefabs from a <see cref="ObjectPool{T}"/>. 
        /// </summary>
        ///
        /// <remarks>
        /// The logic of <see cref="GetProjectile"/> is different from <see cref="ChangeMagCurrentAmount"/>. This function gets
        /// the prefabs that are going to be used in scene. But <see cref="ChangeMagCurrentAmount"/> decrease the number of
        /// projectiles in the magazine. For example, if there is a shotgun, everytime it shots, <see cref="MagCurrentAmount"/>
        /// will reduce by 1, but multiple prefabs of bullet will be got through this function. Therefore, it is best
        /// to use <see cref="GetProjectile"/> and <see cref="ChangeMagCurrentAmount"/> together in situations like
        /// the weapon shoot. 
        /// </remarks>
        /// 
        /// <param name="amount">The amount of projectiles to get.</param>
        /// <param name="projectiles">An array of <see cref="Projectile"/> that contains the desired amount of projectiles.</param>
        /// 
        /// <returns><c>true</c> if there is sufficient amount of projectiles to get. Otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="ArgumentException">If <paramref name="amount"/> is negative.</exception>
        public bool GetProjectile(int amount, out Projectile[] projectiles)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Cannot get a negative amount of projectile. {nameof(amount)}: {amount}.");
            }
            
            if (MagCurrentAmount == 0 || amount > MagCurrentAmount)
            {
                projectiles = Array.Empty<Projectile>();
                return false;
            }
            
            projectiles = new Projectile[amount];
            for (int i = 0; i < amount; i++)
            {
                projectiles[i] = _projectilePool.Get();
            }

            return true;
        }

        /// <summary>
        /// <para>
        /// Reload the magazine.
        /// </para>
        ///
        /// <para>
        /// If the <see cref="ReserveSize"/> is 0, or <see cref="MagCurrentAmount"/> equals to <see cref="MagSize"/>,
        /// then reload will not be performed, meaning <see cref="OnReload"/> will not be triggered.
        /// </para>
        /// </summary>
        /// 
        /// <returns>
        /// <c>true</c> if a reload is successfully performed. Otherwise, <c>false</c>.
        /// </returns>
        public bool Reload()
        {
            if (ReserveCurrentAmount == 0 || MagCurrentAmount == MagSize)
            {
                return false;
            }
            
            int amountBeforeReload = MagSize;
            
            int amountToFill = MagSize - MagCurrentAmount;
            int actualAmountGetFromReserve = ReserveCurrentAmount >= amountToFill ? amountToFill : ReserveCurrentAmount;
            
            ChangeMagCurrentAmount(MagSize);
            ChangeReserveCurrentAmount(ReserveCurrentAmount - actualAmountGetFromReserve);

            OnReload?.Invoke(amountBeforeReload, MagSize);
            return true;
        }

        /// <summary>
        /// <para>
        /// Change the <see cref="MagCurrentAmount"/> to <paramref name="newAmount"/>.
        /// </para>
        ///
        /// <para>
        /// If <see cref="newAmount"/> is larger than <see cref="MagSize"/>, the <see cref="MagCurrentAmount"/> will
        /// be set to <see cref="MagSize"/>.
        /// </para>
        /// </summary>
        /// 
        /// <param name="newAmount">The new amount that <see cref="MagCurrentAmount"/> will be.
        /// This has to be a non-negative number.</param>
        /// 
        /// <exception cref="ArgumentException">If <paramref name="newAmount"/> is negative.</exception>
        public void ChangeMagCurrentAmount(int newAmount)
        {
            if (newAmount < 0)
            {
                throw new ArgumentException(
                    $"Cannot change {nameof(MagCurrentAmount)} to a negative number. {nameof(newAmount)}: " +
                    $"{newAmount}.");
            }
            
            int magAmountBeforeChange = MagCurrentAmount;
            MagCurrentAmount = newAmount > MagSize ? MagSize : newAmount;

            OnMagCurrentAmountChange?.Invoke(magAmountBeforeChange, MagCurrentAmount);
        }

        /// <summary>
        /// <para>
        /// Change the <see cref="ReserveCurrentAmount"/> to <paramref name="newAmount"/>.
        /// </para>
        ///
        /// <para>
        /// If <see cref="newAmount"/> is larger than <see cref="ReserveSize"/>, the <see cref="ReserveCurrentAmount"/> will
        /// be set to <see cref="ReserveSize"/>.
        /// </para>
        /// </summary>
        /// 
        /// <param name="newAmount">The new amount that <see cref="ReserveCurrentAmount"/> will be.
        /// This has to be a non-negative number.</param>
        /// 
        /// <exception cref="ArgumentException">If <paramref name="newAmount"/> is negative.</exception>
        public void ChangeReserveCurrentAmount(int newAmount)
        {
            if (newAmount < 0)
            {
                throw new ArgumentException(
                    $"Cannot change {nameof(ReserveCurrentAmount)} to a negative number. {nameof(newAmount)}: " +
                    $"{newAmount}.");
            }
            
            int reserveAmountBeforeChange = ReserveCurrentAmount;
            ReserveCurrentAmount = newAmount > ReserveSize ? ReserveSize : newAmount;

            OnReserveCurrentAmountChange?.Invoke(reserveAmountBeforeChange, ReserveCurrentAmount);
        }

        /// <summary>
        /// Change the <see cref="MagSize"/> to <paramref name="newSize"/>.
        /// </summary>
        /// 
        /// <param name="newSize">The new amount that <see cref="MagSize"/> will be.
        /// This has to be a non-negative number.</param>
        /// 
        /// <exception cref="ArgumentException">If <paramref name="newSize"/> is negative.</exception>
        public void ChangeMagSize(int newSize)
        {
            if (newSize < 0)
            {
                throw new ArgumentException(
                    $"Cannot change {nameof(MagSize)} to a negative number. {nameof(newSize)}: " +
                    $"{newSize}.");
            }
            
            int magSizeBeforeChange = MagSize;
            MagSize = newSize;
            
            OnMagSizeChange?.Invoke(magSizeBeforeChange, MagSize);
        }

        /// <summary>
        /// Change the <see cref="ReserveSize"/> to <paramref name="newSize"/>.
        /// </summary>
        /// 
        /// <param name="newSize">The new amount that <see cref="ReserveSize"/> will be.
        /// This has to be a non-negative number.</param>
        /// 
        /// <exception cref="ArgumentException">If <paramref name="newSize"/> is negative.</exception>
        public void ChangeReserveSize(int newSize)
        {
            if (newSize < 0)
            {
                throw new ArgumentException(
                    $"Cannot change {nameof(ReserveSize)} to a negative number. {nameof(newSize)}: " +
                    $"{newSize}.");
            }
            
            int reserveSizeBeforeChange = ReserveSize;
            ReserveSize = newSize;
            
            OnReserveSizeChange?.Invoke(reserveSizeBeforeChange, ReserveSize);
        }
        
        #endregion
        
        #region Object Pool Functions

        private Projectile OnCreateProjectile()
        {
            RangeWeapon rangeWeapon = (RangeWeapon)SourceWeapon;
            Transform sourceWeaponTransform = SourceWeapon.transform;
            Projectile projectile = Object.Instantiate(rangeWeapon.Projectile, sourceWeaponTransform.position,
                quaternion.identity, sourceWeaponTransform);
            
            projectile.gameObject.SetActive(false);
            projectile.Init(SourceWeapon.gameObject);
            return projectile;
        }

        private void OnGetProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private void OnReleaseProjectile(Projectile projectile)
        {
            projectile.Reset();
            projectile.gameObject.SetActive(false);
        }

        private void OnDestroyProjectile(Projectile projectile)
        {
            Object.Destroy(projectile.gameObject);
        }

        #endregion
    }
}