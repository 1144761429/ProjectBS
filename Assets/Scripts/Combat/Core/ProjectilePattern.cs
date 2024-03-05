using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Core
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> that represent how a <see cref="IProjectileLauncher"/> shoot.
    ///
    /// This includes, by tapping the trigger once:
    /// 
    /// <list type="number">
    ///     <item>The amount of rounds of <see cref="Projectile"/> will be shot.</item>
    ///     <item>The interval between each round of <see cref="Projectile"/>.</item>
    ///     <item>The direction of each round of <see cref="Projectile"/>.</item>
    ///     <item>The amount of <see cref="Projectile"/> each round has.</item>
    ///     <item>The interval between each <see cref="Projectile"/>.</item>
    ///     <item>The direction of each <see cref="Projectile"/>, based on the direction of the round.</item>
    ///     <item>The type of each <see cref="Projectile"/>.</item>
    ///     <item>The speed of each <see cref="Projectile"/>.</item>
    /// </list>
    /// </summary>
    [CreateAssetMenu(fileName = "NewProjectilePattern", menuName = "Combat/Projectile Pattern")]
    public class ProjectilePattern : ScriptableObject
    {
        /// <summary>
        /// The <see cref="ProjectilePatternRound"/>s of this <see cref="ProjectilePattern"/>.
        /// </summary>
        public List<ProjectilePatternRound> Rounds;

        /// <summary>
        /// The amount of <see cref="ProjectilePatternRound"/> in this <see cref="ProjectilePattern"/>.
        /// </summary>
        public int RoundAmount => Rounds.Count;
        
        //TODO: add functions that set speed, offset, projectile type in runtime.
    }

    /// <summary>
    /// A struct that represents a round of <see cref="ProjectilePattern"/>.
    /// See <see cref="ProjectilePattern"/> for more details about round.
    /// </summary>
    [Serializable]
    public struct ProjectilePatternRound
    {
        /// <summary>
        /// The time that will be waited before the next <see cref="ProjectilePatternRound"/> is shot.
        /// </summary>
        public float IntervalBeforeNext;

        /// <summary>
        /// The direction of where this <see cref="ProjectilePatternRound"/> is shot based on
        /// <see cref="IProjectileLauncher"/>'s direction.
        /// </summary>
        public Vector3 GeneralDirection;

        /// <summary>
        /// The offset of the <see cref="Projectile"/> when being shot based on <see cref="GeneralDirection"/>.
        /// In other words, the random spread.
        /// </summary>
        public Vector2 DirectionOffset;
        
        /// <summary>
        /// The <see cref="SingleProjectilePattern"/>s in this <see cref="ProjectilePatternRound"/>.
        /// </summary>
        public List<SingleProjectilePattern> Projectiles;
    }

    /// <summary>
    /// A struct that represents the behaviour of a single <see cref="Projectile"/> in a <see cref="ProjectilePatternRound"/>.
    /// </summary>
    [Serializable]
    public struct SingleProjectilePattern
    {
        /// <summary>
        /// The time that will be waited before the next <see cref="Projectile"/> is shot.
        /// </summary>
        public float IntervalBeforeNext;
        
        /// <summary>
        /// The direction of where this <see cref="Projectile"/> is shot based on
        /// <see cref="ProjectilePatternRound.GeneralDirection"/> of <see cref="ProjectilePatternRound"/>.
        /// </summary>
        public Vector3 GeneralDirection;

        /// <summary>
        /// The offset of the <see cref="Projectile"/> when being shot based on <see cref="GeneralDirection"/>.
        /// In other words, the random spread.
        /// </summary>
        public Vector2 DirectionOffset;
        
        /// <summary>
        /// The type of <see cref="Projectile"/> to shot.
        /// </summary>
        public Projectile Projectile;

        /// <summary>
        /// The speed of the <see cref="Projectile"/>.
        /// </summary>
        public float Speed;
    }
}