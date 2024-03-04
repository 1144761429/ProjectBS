namespace Utilities.StackableElement.Core
{
    /// <summary>
    /// An interface that represents a thing that can have stacks.
    /// </summary>
    /// 
    /// <example>
    /// <list type="number">
    ///     <item>A buff that increase damage by 10% with maximum stack of 3.</item>
    ///     <item>An item that is stored in inventory that can pile up to 64.</item>
    /// </list>
    /// </example>
    public interface IStackable
    {
        #region Properties
        
        /// <summary>
        /// The stack number.
        /// </summary>
        int Stack { get; }
        
        /// <summary>
        /// The minimum stack can have.
        /// </summary>
        int MinStack { get; }
        
        /// <summary>
        /// The maximum stack can have.
        /// </summary>
        int MaxStack { get; }
        
        /// <summary>
        /// If this <see cref="IStackable"/> object is currently operating like a switcher,
        /// meaning it have a maximum stack of 1 and minimum stack of 0. 
        /// </summary>
        bool IsSwitcher { get; }
        
        /// <summary>
        /// If the <see cref="Stack"/> of this <see cref="IStackable"/> object is currently unchangeable.
        /// </summary>
        bool IsFrozen { get; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Increase the <see cref="Stack"/> by <see cref="amount"/>.
        /// </summary>
        ///
        /// <example>
        /// Given the <see cref="MaxStack"/> is -4, but the actual <see cref="Stack"/> after increment is -1.
        /// Then, the overflow amount should be 3 instead of -3. 
        /// </example>
        /// 
        /// <param name="amount">The amount to increase. This should be non-negative.</param>
        /// <param name="overflowAmount">The amount that exceeds the maximum amount. This is non-negative.
        /// If this is 0, then there is no overflow.</param>
        void IncreaseStack(int amount, out int overflowAmount);
        
        /// <summary>
        /// Decrease the <see cref="Stack"/> by <see cref="amount"/>.
        /// </summary>
        ///
        /// <example>
        /// Given the <see cref="MinStack"/> is -2, but the actual <see cref="Stack"/> after decrement is -5.
        /// Then, the overflow amount should be 3 instead of -3. 
        /// </example>
        /// 
        /// <param name="amount">The amount to decrease. This should be non-negative.</param>
        /// <param name="overflowAmount">The amount that exceeds the minimum amount. This is non-negative.
        /// If this is 0, then there is no overflow.</param>
        void DecreaseStack(int amount, out int overflowAmount);

        /// <summary>
        /// Reset the stack.
        /// </summary>
        void Reset();
        
        #endregion
    }
}