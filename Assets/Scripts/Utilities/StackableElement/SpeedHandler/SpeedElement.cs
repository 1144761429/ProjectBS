using System;
using Utilities.StackableElement.Core;

namespace Utilities.StackableElement.SpeedHandler
{
    /// <summary>
    /// A <see cref="SpeedElement"/> is the basic unit of speed.
    /// It consists of a <see cref="Value"/>, how fast the speed is;
    /// and a <see cref="Stack"/>.
    /// </summary>
    public class SpeedElement : IStackable
    {
        #region Fields and Properties
        
        /// <summary>
        /// The value of the speed (how fast the speed it).
        /// </summary>
        public float Value { get; private set; }
        
        /// <inheritdoc/>
        public int Stack { get; private set; }

        /// <summary>
        /// The overall value, <see cref="Value"/> * <see cref="Stack"/>, of this <see cref="SpeedElement"/>.
        /// </summary>
        public float OverallValue => Value * Stack;
        
        /// <summary>
        /// <para>
        /// The default stack of this <see cref="SpeedElement"/>. It has to be within the range of
        /// <see cref="MinStack"/> and <see cref="MaxStack"/>.
        /// </para>
        /// 
        /// <para>
        /// The initial stack of this <see cref="SpeedElement"/> will be set to this when initialized.
        /// </para>
        ///
        /// <para>
        /// The stack of this <see cref="SpeedElement"/> will be set to this when calling <see cref="Reset"/>.
        /// </para>
        /// </summary>
        public int DefaultStack { get; private set; }
        
        /// <inheritdoc/>
        public int MinStack { get; private set; }
        
        /// <inheritdoc/>
        public int MaxStack { get; private set; }
        
        /// <inheritdoc/>
        public bool IsSwitcher { get; private set; }
        
        /// <inheritdoc/>
        public bool IsFrozen { get; private set; }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// An constructor that initialize a <see cref="SpeedElement"/> and set its <see cref="Stack"/> to
        /// <paramref name="defaultStack"/>.
        /// </summary>
        /// 
        /// <param name="value">The value of the speed.</param>
        /// <param name="defaultStack">The <see cref="DefaultStack"/>.</param>
        /// <param name="minStack">The <see cref="MinStack"/>.</param>
        /// <param name="maxStack">The <see cref="MaxStack"/>.</param>
        /// <param name="isSwitcher">If this <see cref="SpeedElement"/> is currently operating like a switcher,
        /// meaning it have a <see cref="MaxStack"/> of 1 and <see cref="MinStack"/> of 0.</param>
        /// <param name="isFrozen">If the <see cref="Stack"/> of this <see cref="SpeedElement"/> is currently unchangeable.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// <list type="number">
        ///     <item>If the <paramref name="minStack"/> is larger than <paramref name="maxStack"/>.</item>
        ///     <item>If <paramref name="defaultStack"/> is not in the range of <paramref name="minStack"/> and <paramref name="maxStack"/>.</item>
        ///     <item>If <paramref name="isSwitcher"/> is <c>true</c> but the <paramref name="minStack"/> is no 0 or <paramref name="maxStack"/> is not 1.</item>
        /// </list>
        /// </exception>
        public SpeedElement(float value, int defaultStack, int minStack, int maxStack, bool isSwitcher = false,
            bool isFrozen = false)
        {
            if (minStack > maxStack)
            {
                throw new ArgumentException(
                    $"Trying to set the {nameof(MinStack)} of a {typeof(SpeedElement)} larger than {nameof(MaxStack)} of that." +
                    $"{nameof(MinStack)}: {minStack}, {nameof(MaxStack)}: {maxStack}.");
            }

            if (defaultStack < minStack || defaultStack > maxStack)
            {
                throw new ArgumentException(
                    $"When initializing a {typeof(SpeedElement)}, the {nameof(defaultStack)} has to be within " +
                    $"the range of {nameof(MinStack)} and {nameof(MaxStack)}." +
                    $"{nameof(DefaultStack)}: {defaultStack}, {nameof(MinStack)}: {minStack}, {nameof(MaxStack)}: {maxStack}.");
            }

            if (isSwitcher && (minStack != 0 || maxStack != 1))
            {
                throw new ArgumentException(
                    $"When initializing a {typeof(SpeedElement)}, it is set to be a switcher, " +
                    $"but its {nameof(MinStack)} and {nameof(MaxStack)} are not set to 0 and 1." +
                    $"{nameof(MinStack)}: {minStack}, {nameof(MaxStack)}: {maxStack}.");
            }

            Value = value;
            DefaultStack = defaultStack;
            MinStack = minStack;
            MaxStack = maxStack;
            IsSwitcher = isSwitcher;
            IsFrozen = isFrozen;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If an overflow happens, the Stack will be set to <see cref="MaxStack"/>.
        /// </remarks>
        public void IncreaseStack(int amount, out int overflowAmount)
        {
            int actualStack = Stack + amount;
            
            overflowAmount = actualStack > MaxStack? actualStack - MaxStack : 0;
            Stack = overflowAmount == 0 ? actualStack : MaxStack;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If an overflow happens, the Stack will be set to <see cref="MinStack"/>.
        /// </remarks>
        public void DecreaseStack(int amount, out int overflowAmount)
        {
            int actualStack = Stack - amount;
            
            overflowAmount = actualStack < MinStack? MinStack - actualStack: 0;
            Stack = overflowAmount == 0 ? actualStack : MinStack;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Stack = DefaultStack;
        }
        
        #endregion
    }
}