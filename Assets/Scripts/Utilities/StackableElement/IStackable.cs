namespace Utilities.StackableElement
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
        /// If the <see cref="Stack"/> of this <see cref="IStackable"/> object is currently Changeable.
        /// </summary>
        bool IsFrozen { get; }

        /// <summary>
        /// Increase the <see cref="Stack"/> by <see cref="amount"/>.
        /// </summary>
        void IncreaseStack(int amount);
        
        /// <summary>
        /// Decrease the <see cref="Stack"/> by <see cref="amount"/>.
        /// </summary>
        void DecreaseStack(int amount);

        /// <summary>
        /// Reset the stack.
        /// </summary>
        void Reset();
    }
}