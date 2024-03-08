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
    [CreateAssetMenu(fileName = "NewShootPattern", menuName = "Combat/Shoot Pattern")]
    public class ShootPattern : ScriptableObject
    {
        /// <summary>
        /// The <see cref="ShootPatternRound"/>s of this <see cref="ShootPattern"/>.
        /// </summary>
        public List<ShootPatternRound> Rounds { get; private set; }

        /// <summary>
        /// The amount of <see cref="ShootPatternRound"/> in this <see cref="ShootPattern"/>.
        /// </summary>
        public int RoundCount => Rounds.Count;

        /// <summary>
        /// The total amount of <see cref="Projectile"/> that will be shot if execute this <see cref="ShootPattern"/>.
        /// </summary>
        public int ProjectileCount
        {
            get
            {
                int sum = 0;
                
                foreach (ShootPatternRound round in Rounds)
                {
                    sum += round.ProjectileCount;
                }

                return sum;
            }
        }
        
        #region Set ShootPatternRound

        /// <summary>
        /// <para>
        /// Set the <see cref="ShootPatternRound.IntervalBeforeNext"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="interval"/>.
        /// </para>
        /// 
        /// <para>
        /// <paramref name="interval"/> cannot be negative.
        /// </para>
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number.</param>
        /// <param name="interval">The new interval.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> does not exist.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="interval"/> is negative.
        /// </exception>
        public void SetRoundInterval(int roundIndex, float interval)
        {
            IsValidIndex(roundIndex);

            if (interval < 0)
            {
                throw new ArgumentException($"{nameof(interval)} cannot be set to negative. " +
                                            $"{nameof(ShootPattern)}, {nameof(roundIndex)}: {roundIndex}, " +
                                            $"{nameof(interval)}: {interval}.");
            }
            
            Rounds[roundIndex].IntervalBeforeNext = interval;
        }

        /// <summary>
        /// <para>
        /// Set the <see cref="ShootPatternRound.GeneralDirection"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="direction"/>.
        /// </para>
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number.</param>
        /// <param name="direction">The new direction.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> does not exist.
        /// </exception>
        public void SetRoundGeneralDirection(int roundIndex, Vector3 direction)
        {
            IsValidIndex(roundIndex);
            
            Rounds[roundIndex].GeneralDirection = direction;
        }
        
        /// <summary>
        /// <para>
        /// Set the <see cref="ShootPatternRound.DirectionOffset"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="offset"/>.
        /// </para>
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number.</param>
        /// <param name="offset">The new offset.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> does not exist.
        /// </exception>
        public void SetRoundDirectionOffset(int roundIndex, Vector2 offset)
        {
            IsValidIndex(roundIndex);
            
            Rounds[roundIndex].GeneralDirection = offset;
        }
        
        #endregion
        
        #region Set SingleProjectilePattern
        
        /// <summary>
        /// Set the <see cref="projectileIndex"/>th <see cref="SingleProjectilePattern.Speed"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="speed"/>.
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number that the <see cref="Projectile"/> is in.</param>
        /// <param name="projectileIndex">The index of the <see cref="Projectile"/> in that round.</param>
        /// <param name="speed">The new speed.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> projectile <paramref name="projectileIndex"/>> does not exist.
        /// </exception>
        public void SetProjectileSpeed(int roundIndex, int projectileIndex, float speed)
        {
            IsValidIndex(roundIndex, projectileIndex);

            Rounds[roundIndex].Projectiles[projectileIndex].Speed = speed;
        }

        /// <summary>
        /// Set the <see cref="projectileIndex"/>th <see cref="SingleProjectilePattern.IntervalBeforeNext"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="interval"/>.
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number that the <see cref="Projectile"/> is in.</param>
        /// <param name="projectileIndex">The index of the <see cref="Projectile"/> in that round.</param>
        /// <param name="interval">The new interval.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> projectile <paramref name="projectileIndex"/>> does not exist.
        /// </exception>
        public void SetProjectileInterval(int roundIndex, int projectileIndex, float interval)
        {
            IsValidIndex(roundIndex, projectileIndex);

            Rounds[roundIndex].Projectiles[projectileIndex].IntervalBeforeNext = interval;
        }
        
        /// <summary>
        /// Set the <see cref="projectileIndex"/>th <see cref="SingleProjectilePattern.GeneralDirection"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="direction"/>.
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number that the <see cref="Projectile"/> is in.</param>
        /// <param name="projectileIndex">The index of the <see cref="Projectile"/> in that round.</param>
        /// <param name="direction">The new direction.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> projectile <paramref name="projectileIndex"/>> does not exist.
        /// </exception>
        public void SetProjectileGeneralDirection(int roundIndex, int projectileIndex, Vector3 direction)
        {
            IsValidIndex(roundIndex, projectileIndex);

            Rounds[roundIndex].Projectiles[projectileIndex].GeneralDirection = direction;
        }
        
        /// <summary>
        /// Set the <see cref="projectileIndex"/>th <see cref="SingleProjectilePattern.DirectionOffset"/> in round
        /// <paramref name="roundIndex"/> to <paramref name="offset"/>.
        /// </summary>
        /// 
        /// <param name="roundIndex">The round number that the <see cref="Projectile"/> is in.</param>
        /// <param name="projectileIndex">The index of the <see cref="Projectile"/> in that round.</param>
        /// <param name="offset">The new offset.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> projectile <paramref name="projectileIndex"/>> does not exist.
        /// </exception>
        public void SetProjectileDirectionOffset(int roundIndex, int projectileIndex, Vector2 offset)
        {
            IsValidIndex(roundIndex, projectileIndex);

            Rounds[roundIndex].Projectiles[projectileIndex].DirectionOffset = offset;
        }

        #endregion
        
        /// <summary>
        /// Get the the number of how many <see cref="Projectile"/> each <see cref="ShootPatternRound"/> has.
        /// </summary>
        /// 
        /// <returns>An array that contains the number of <see cref="Projectile"/>s each
        /// <see cref="ShootPatternRound"/> has.</returns>
        private int[] GetProjectileAmountInEachRound()
        {
            int[] result = new int[RoundCount];

            for (int i = 0; i < RoundCount; i++)
            {
                result[i] = Rounds[i].Projectiles.Count;
            }

            return result;
        }
        
        /// <summary>
        /// Check if an round index <paramref name="roundIndex"/> is within the range of 0-<see cref="RoundCount"/>,
        /// inclusive. The index starts at 0 and ends at <see cref="RoundCount"/> - 1. If the index in invalid, an
        /// <see cref="ArgumentOutOfRangeException"/> will be thrown.
        /// </summary>
        ///
        /// <remarks>
        /// If <paramref name="projectileIndex"/> is 0, and <paramref name="roundIndex"/> is valid, then this is always
        /// <c>true</c>.
        /// </remarks>
        /// 
        /// <param name="roundIndex">The round index to validate.</param>
        /// <param name="projectileIndex">The projectile index, in round <paramref name="roundIndex"/>, to validate.</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// If round <paramref name="roundIndex"/> projectile <paramref name="projectileIndex"/>> does not exist.
        /// </exception>
        private void IsValidIndex(int roundIndex, int projectileIndex = 0)
        {
            if (roundIndex >= Rounds.Count || roundIndex < 0)
            {
                string message = $"{typeof(ShootPattern)} {nameof(ShootPattern)} " +
                                 $"have {RoundCount} rounds with projectile numbers " +
                                 $"{GetProjectileAmountInEachRound()}, but you are trying to access round: {roundIndex} " +
                                 $"{typeof(Projectile)}: {projectileIndex}.";
                
                throw new ArgumentOutOfRangeException(message);
            }
        }
    }

    /// <summary>
    /// A class that represents a round of <see cref="ShootPattern"/>.
    /// See <see cref="ShootPattern"/> for more details about round.
    /// </summary>
    [Serializable]
    public class ShootPatternRound
    {
        /// <summary>
        /// The time that will be waited before the next <see cref="ShootPatternRound"/> is shot.
        /// </summary>
        public float IntervalBeforeNext;

        /// <summary>
        /// The direction of where this <see cref="ShootPatternRound"/> is shot based on
        /// <see cref="IProjectileLauncher"/>'s direction.
        /// </summary>
        public Vector3 GeneralDirection;

        /// <summary>
        /// The offset of the <see cref="Projectile"/> when being shot based on <see cref="GeneralDirection"/>.
        /// In other words, the random spread.
        /// </summary>
        public Vector2 DirectionOffset;
        
        /// <summary>
        /// The <see cref="SingleProjectilePattern"/>s in this <see cref="ShootPatternRound"/>.
        /// </summary>
        public List<SingleProjectilePattern> Projectiles;

        /// <summary>
        /// The amount of <see cref="Projectiles"/> in a round.
        /// </summary>
        public int ProjectileCount => Projectiles.Count;
    }

    /// <summary>
    /// A class that represents the behaviour of a single <see cref="Projectile"/> in a <see cref="ShootPatternRound"/>.
    /// </summary>
    [Serializable]
    public class SingleProjectilePattern
    {
        /// <summary>
        /// The time that will be waited before the next <see cref="Projectile"/> is shot.
        /// </summary>
        public float IntervalBeforeNext;
        
        /// <summary>
        /// The direction of where this <see cref="Projectile"/> is shot based on
        /// <see cref="ShootPatternRound.GeneralDirection"/> of <see cref="ShootPatternRound"/>.
        /// </summary>
        public Vector3 GeneralDirection;

        /// <summary>
        /// The offset of the <see cref="Projectile"/> when being shot based on <see cref="GeneralDirection"/>.
        /// In other words, the random spread.
        /// </summary>
        public Vector2 DirectionOffset;

        /// <summary>
        /// The speed of the <see cref="Projectile"/>.
        /// </summary>
        public float Speed;
    }
}