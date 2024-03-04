using JetBrains.Annotations;

namespace Utilities.StackableElement.Core
{
    /// <summary>
    /// An object that stores information of overflow of an increment or decrement on an <see cref="IStackable"/>.
    /// </summary>
    public struct StackableElementOverflowInfo
    {
        /// <summary>
        /// The ID/Name of the <see cref="IStackable"/>.
        /// </summary>
        ///
        /// <remarks>
        /// This will be null if the <see cref="StackableElement"/> is not in a <see cref="StackableElementHandler{TID,TStackable}"/>.
        /// </remarks>
        [CanBeNull]
        public string ID { get; private set; }
        
        /// <summary>
        /// The amount of stack that was attempted to change.
        /// </summary>
        public int AttemptedAmountToChange { get; private set; }
        
        /// <summary>
        /// The amount of stacks that overflows.
        /// </summary>
        public int OverflowAmount { get; private set; }
        
        /// <summary>
        /// The name/string representation of the <see cref="StackableElementHandler{TID,TStackable}"/> that the
        /// <see cref="IStackable"/> of ID <see cref="ID"/> belongs to.
        /// </summary>
        ///
        /// <remarks>
        /// This will be null if the <see cref="StackableElement"/> is not in a <see cref="StackableElementHandler{TID,TStackable}"/>.
        /// </remarks>
        public string StackableElementHandlerName { get; private set; }
        
        /// <summary>
        /// An constructor that sets the <see cref="ID"/>, <see cref="OverflowAmount"/>, and <see cref="StackableElementHandlerName"/>;
        /// </summary>
        /// 
        /// <param name="id">The ID/Name of the <see cref="IStackable"/>.</param>
        /// <param name="attemptedAmountToChange">
        /// The amount of increment of decrement that was attempted.
        /// If this is positive, then an increment was attempted.
        /// If this is negative, then an decrement was attempted.
        /// </param>
        /// <param name="stackableElementHandlerName">
        /// The name/string representation of the <see cref="StackableElementHandler{TID,TStackable}"/> that the
        /// <see cref="IStackable"/> of ID <see cref="ID"/> belongs to.
        /// </param>
        /// <param name="overflowAmount">The amount of stacks that overflows.</param>
        public StackableElementOverflowInfo(string id, int attemptedAmountToChange, string stackableElementHandlerName, int overflowAmount)
        {
            ID = id;
            AttemptedAmountToChange = attemptedAmountToChange;
            StackableElementHandlerName = stackableElementHandlerName; 
            OverflowAmount = overflowAmount;
        }
    }
}