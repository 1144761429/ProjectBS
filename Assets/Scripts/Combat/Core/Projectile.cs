using System;
using System.Linq;
using Combat.Core.EventArguments;
using UnityEngine;

namespace Combat.Core
{
    /// <summary>
    /// A class that represents a projectile can be launched by <see cref="IProjectileLauncher"/> object.
    /// </summary>
    public abstract class Projectile : MonoBehaviour, IDamagingEntity
    {
        #region Events

        /// <summary>
        /// An event that will be triggered when this <see cref="Projectile"/> hit something.
        /// </summary>
        public event EventHandler<OnDamagingEntityHitEventArgs> OnHit;
        
        /// <summary>
        /// An event that will be triggered after <see cref="OnHit"/> is triggered.
        /// </summary>
        public event EventHandler<OnDamagingEntityHitEventArgs> AfterHit;

        #endregion

        #region Fields and Properties
        
        /// <summary>
        /// If this <see cref="Projectile"/> is moving.
        /// </summary>
        public bool Traversing { get; private set; }
        
        /// <summary>
        /// The total distance this <see cref="Projectile"/> has moved.
        /// </summary>
        public float TraveledDistance { get; private set;}
        
        /// <summary>
        /// The maximum distance this <see cref="Projectile"/> can move.
        /// Once reaches the maximum distance, this <see cref="Projectile"/>
        /// will be destroyed.
        /// </summary>
        public float MaxTravelDistance { get; private set;}

        /// <summary>
        /// The <see cref="LayerMask"/>s that this <see cref="Projectile"/> can collide with.
        /// </summary>
        public LayerMask[] CollidableLayer { get; protected set; }
        
        /// <summary>
        /// The tags that this <see cref="Projectile"/> will ignore when colliding.
        /// </summary>
        public string[] TagsToIgnore { get; protected set; }
        
        /// <summary>
        /// <para>
        /// If this <see cref="Projectile"/> has been initialized.
        /// </para>
        ///
        /// <para>
        /// A <see cref="Projectile"/> can only be shot/used if it has been initialized.
        /// Call <see cref="Init"/> to initialize this <see cref="Projectile"/>.
        /// </para>
        /// </summary>
        private bool _initialized;
        
        #endregion
        
        #region Monobehavior Functions

        private void FixedUpdate()
        {
            CheckInitialized();
            
            if (Traversing)
            {
                OnTraveling();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckInitialized();
            
            if (CollidableLayer.Contains(other.gameObject.layer) && !TagsToIgnore.Contains(other.gameObject.tag))
            {
                OnDamagingEntityHitEventArgs eventArgs = new OnDamagingEntityHitEventArgs(this, other);
                OnHit?.Invoke(this, eventArgs);
                AfterHit?.Invoke(this, eventArgs);
            }
        }

        #endregion
        
        #region Public Functions
        
        /// <summary>
        /// Initialize this <see cref="Projectile"/>.
        /// </summary>
        /// 
        /// <param name="source">The <see cref="GameObject"/> that launches this <see cref="Projectile"/>.</param>
        /// <param name="traverseAfterInit">If true, then the <see cref="Traversing"/> of this <see cref="Projectile"/>
        /// will be set to true. Otherwise, <see cref="Traversing"/> will be false.</param>
        public void Init(GameObject source, bool traverseAfterInit = false)
        {
            Source = source;
            Rb = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
            Traversing = traverseAfterInit;
            
            _initialized = true;
        }

        /// <summary>
        /// Reset this <see cref="Projectile"/> by setting <see cref="Traversing"/> to false and set
        /// <see cref="TraveledDistance"/> to 0.
        /// </summary>
        ///
        /// <exception cref="Exception">If this <see cref="Projectile"/> is not initialized.</exception>
        public void Reset()
        {
            CheckInitialized();
            
            Traversing = false;
            TraveledDistance = 0;
        }
        
        #endregion

        #region Private Functions

        /// <summary>
        /// Check if this <see cref="Projectile"/> is initialized. If not, throw an exception.
        /// </summary>
        /// <exception cref="Exception">If this <see cref="Projectile"/> is not initialized.</exception>
        private void CheckInitialized()
        {
            if (!_initialized)
            {
                throw new Exception($"{nameof(Projectile)}: {gameObject.name} is not initialized before use.");
            }
        }
        
        /// <summary>
        /// As this <see cref="Projectile"/> is traveling, increase its <see cref="TraveledDistance"/> according to its
        /// velocity. When reaching <see cref="MaxTravelDistance"/>, destroy this <see cref="Projectile"/> as wel as its
        /// parent <see cref="GameObject"/>.
        /// </summary>
        private void OnTraveling()
        {
            TraveledDistance += Time.fixedDeltaTime * Rb.velocity.magnitude;

            if (TraveledDistance >= MaxTravelDistance)
            {
                Destroy(gameObject);
            }
        }
        
        #endregion
        
        #region IDamagingEntity
        
        /// <inheritdoc/>
        public GameObject Source { get; private set; }
        
        /// <inheritdoc/>
        public Rigidbody Rb { get; private set; }
        
        /// <inheritdoc/>
        public Collider Collider { get; private set; }
        
        /// <inheritdoc/>
        public void DoDamage(IDamageable damageable, float amount)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}
