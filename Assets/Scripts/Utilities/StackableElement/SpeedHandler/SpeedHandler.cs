using System;
using System.Collections.Generic;
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
        public StackableElementHandler<TID, SpeedElement> AdditiveBonus { get; private set; }
        
        /// <summary>
        /// A <see cref="StackableElementHandler{TID,TStackable}"/> that stores all the <see cref="SpeedElement"/> that
        /// falls into the multiplier section when calculating the speed.
        /// </summary>
        public StackableElementHandler<TID, SpeedElement> Multiplier { get; private set; }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// An constructor that creates an empty <see cref="SpeedHandler{TID}"/>.
        /// </summary>
        public SpeedHandler()
        {
            AdditiveBonus = new StackableElementHandler<TID, SpeedElement>();
            Multiplier = new StackableElementHandler<TID, SpeedElement>();
        }
    
        /// <summary>
        /// Get the sum of the value of all the <see cref="SpeedElement"/> in this handler.
        /// </summary>
        /// <returns></returns>
        public float GetSpeed()
        {
            return GetAdditiveBonusSum() * (1 + GetMultiplierSum());
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="AdditiveBonus"/> section.
        /// </summary>
        /// 
        /// <returns>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="AdditiveBonus"/> section.
        /// </returns>
        private float GetAdditiveBonusSum()
        {
            float sum = 0;

            foreach (KeyValuePair<TID, SpeedElement> speedElement in AdditiveBonus)
            {
                sum += speedElement.Value.OverallValue;
            }

            return sum;
        }
        
        /// <summary>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="Multiplier"/> section.
        /// </summary>
        /// 
        /// <returns>
        /// Get the sum of <see cref="SpeedElement.OverallValue"/> of all the <see cref="SpeedElement"/> in
        /// the <see cref="Multiplier"/> section.
        /// </returns>
        private float GetMultiplierSum()
        {
            float sum = 0;

            foreach (KeyValuePair<TID, SpeedElement> speedElement in Multiplier)
            {
                sum += speedElement.Value.OverallValue;
            }

            return sum;
        }
        
        #endregion
    }
}
