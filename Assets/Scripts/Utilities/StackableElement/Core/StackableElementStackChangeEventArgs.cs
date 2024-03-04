using System;

namespace Utilities.StackableElement.Core
{
    /// <summary>
    /// An <see cref="EventArgs"/> that is used when a <see cref="IStackable"/> object attempts to change its
    /// <see cref="IStackable.Stack"/> in a <see cref="StackableElementHandler{TID,TStackable}"/>.
    ///
    /// <remarks>
    /// This includes the case
    /// that adding a new <see cref="IStackable"/> object with initial <see cref="IStackable.Stack"/> of 0, or remove a
    /// <see cref="IStackable"/> object with current <see cref="IStackable.Stack"/> of 0.
    /// </remarks>
    /// </summary>
    public class StackableElementStackChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The ID of the <see cref="IStackable"/> object in the in <see cref="StackableElementHandler{TID,TStackable}"/>.
        /// </summary>
        public readonly Enum ID;

        /// <summary>
        /// The <see cref="IStackable.Stack"/> amount before the change.
        /// </summary>
        public readonly int StackBeforeChange;
        
        /// <summary>
        /// The <see cref="IStackable.Stack"/> amount after the change.
        /// </summary>
        public readonly int StackAfterChange;

        /// <summary>
        /// The changes of <see cref="IStackable.Stack"/>.
        /// </summary>
        ///
        /// <remarks>
        /// This can be positive or negative, or 0.
        /// </remarks>
        public readonly int StackChange;
        
        /// <summary>
        /// A constructor that create a <see cref="StackableElementStackChangeEventArgs"/> with certain
        /// <see cref="ID"/>, <see cref="StackBeforeChange"/>, and <see cref="StackAfterChange"/>.
        /// </summary>
        /// 
        /// <param name="id">The ID of the <see cref="IStackable"/> object in the in <see cref="StackableElementHandler{TID,TStackable}"/>.</param>
        /// <param name="stackBeforeChange">The <see cref="IStackable.Stack"/> amount before the change.</param>
        /// <param name="stackAfterChange">The <see cref="IStackable.Stack"/> amount after the change.</param>
        public StackableElementStackChangeEventArgs(Enum id, int stackBeforeChange, int stackAfterChange)
        {
            ID = id;
            StackBeforeChange = stackBeforeChange;
            StackAfterChange = stackAfterChange;
            StackChange = StackAfterChange - StackBeforeChange;
        }
    }
}