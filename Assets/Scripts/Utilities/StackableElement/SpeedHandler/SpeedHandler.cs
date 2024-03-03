using System;
using Utilities.StackableElement.Core;

namespace Utilities.StackableElement.SpeedHandler
{
    /// <summary>
    /// <para>
    /// A handler that that dedicates to handle <see cref="SpeedElement"/>.
    /// </para>
    ///
    /// <para>
    /// The formula for calculating speed: Additive Bonus * (1 + Multiplier)
    /// </para>
    /// </summary>
    /// 
    /// <typeparam name="TID">The type of <see cref="Enum"/> that are used to distinguish each
    /// <see cref="SpeedElement"/> in this handler.</typeparam>
    public class SpeedHandler<TID>
    where TID : Enum
    {
        #region Fields and Properties
        
        /// <summary>
        /// A <see cref="StackableElementHandler{TID,TStackable}"/> that stores all the <see cref="SpeedElement"/> that
        /// falls into the additive bonus section when calculating the speed.
        /// </summary>
        private readonly StackableElementHandler<TID, SpeedElement> _additiveBonus;
        
        /// <summary>
        /// A <see cref="StackableElementHandler{TID,TStackable}"/> that stores all the <see cref="SpeedElement"/> that
        /// falls into the multiplier section when calculating the speed.
        /// </summary>
        private readonly StackableElementHandler<TID, SpeedElement> _multiplier;
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// An constructor that creates an empty <see cref="SpeedHandler{TID}"/>.
        /// </summary>
        public SpeedHandler()
        {
            _additiveBonus = new StackableElementHandler<TID, SpeedElement>();
            _multiplier = new StackableElementHandler<TID, SpeedElement>();
        }
    
        /// <summary>
        /// Get the sum of the value of all the <see cref="SpeedElement"/> in this handler.
        /// </summary>
        /// <returns></returns>
        public float GetSpeed()
        {
            return GetAdditiveBonusSum() * GetMultiplierSum();
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="_additiveBonus"/> section.
        /// </summary>
        /// 
        /// <returns>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="_additiveBonus"/> section.
        /// </returns>
        private float GetAdditiveBonusSum()
        {
            float sum = 0;

            foreach (SpeedElement speedElement in _additiveBonus)
            {
                sum += speedElement.OverallValue;
            }

            return sum;
        }
        
        /// <summary>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="_multiplier"/> section.
        /// </summary>
        /// 
        /// <returns>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="_multiplier"/> section.
        /// </returns>
        private float GetMultiplierSum()
        {
            float sum = 0;

            foreach (SpeedElement speedElement in _multiplier)
            {
                sum += speedElement.OverallValue;
            }

            return sum;
        }
        
        #endregion
    }
}
