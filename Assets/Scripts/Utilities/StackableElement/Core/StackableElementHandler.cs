using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Utilities.StackableElement.Core
{
    /// <summary>
    /// A class that handles and organizes a group of <see cref="IStackable"/> object.
    /// </summary>
    ///
    /// <typeparam name="TID">The type of the ID/name of the <see cref="IStackable"/> in this <see cref="StackableElementHandler{TIDEnum,TStackable}"/></typeparam>
    /// <typeparam name="TStackable">The type of <see cref="IStackable"/> that this handler handles.</typeparam>
    public class StackableElementHandler<TID, TStackable> : IEnumerable<TStackable>
        where TID : Enum
        where TStackable : IStackable
    {
        #region Fields and Properties
        
        /// <summary>
        /// The total number of <see cref="TStackable"/> in this handler.
        /// </summary>
        public int Count => _dictionary.Count;
        
        /// <summary>
        /// The total number of <see cref="TStackable"/> that does not have a 0 stack.
        /// </summary>
        public int NonZeroCount { get; private set; }
        
        /// <summary>
        /// A <see cref="Dictionary{TKey,TValue}"/> that maps an ID/name of <see cref="TID"/> to a <see cref="TStackable"/>.
        /// </summary>
        private readonly Dictionary<TID, TStackable> _dictionary;

        #endregion

        #region Public Methods
        
        /// <summary>
        /// A constructor that initializes the <see cref="StackableElementHandler{TID,TStackable}"/>.
        /// </summary>
        public StackableElementHandler()
        {
            _dictionary = new Dictionary<TID, TStackable>();
        }

        /// <summary>
        /// Add a <see cref="TStackable"/> with ID/name of <see cref="TID"/> to this handler.
        /// </summary>
        /// <param name="id">The ID/name of <see cref="stackableElement"/> that distinguish it among other <see cref="TStackable"/>s.</param>
        /// <param name="stackableElement">The <see cref="TStackable"/> that will be stored in this handler.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// The key <paramref name="id"/> already exists.
        /// </exception>
        public void Add(TID id, [NotNull]TStackable stackableElement)
        {
            if (_dictionary.TryAdd(id, stackableElement) && stackableElement.Stack != 0)
            {
                NonZeroCount++;
            }

            throw new ArgumentException($"An {typeof(TStackable)} with ID {id} already exists in this handler.");
        }

        /// <summary>
        /// Completely remove a <see cref="TStackable"/> from this handler.
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// This function does not set the stack of a <see cref="TStackable"/> to 0, but completely removes it.
        /// </para>
        ///
        /// <para>
        /// Therefore, before the next time trying to access the <see cref="TStackable"/> with ID/name of <see cref="TID"/>,
        /// it is required to add that <see cref="TStackable"/>, since that deleted <see cref="TStackable"/> no longer
        /// exists in this handler.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="id">The ID/name of a <see cref="TStackable"/> to delete.</param>
        /// 
        /// <returns>
        /// <c>true</c> if the <see cref="TStackable"/> with ID of <see cref="TID"/> is successfully deleted.
        /// Otherwise, return <c>false</c>.
        /// </returns>
        public bool Delete(TID id)
        {
            if (_dictionary.TryGetValue(id, out TStackable stackableElement) && stackableElement.Stack != 0)
            {
                NonZeroCount--;
            }
            
            return _dictionary.Remove(id);
        }

        /// <summary>
        /// Delete all <see cref="TStackable"/>s in this handler.
        /// </summary>
        public void DeleteAll()
        {
            _dictionary.Clear();
            NonZeroCount = 0;
        }
        
        /// <summary>
        /// Increase the stack number of a <see cref="TStackable"/> with ID <paramref name="id"/> by <paramref name="amount"/>.
        /// </summary>
        /// 
        /// <param name="id">The ID of the <see cref="TStackable"/> to increase stack.</param>
        /// <param name="amount">The amount of stack that will be increased.</param>
        ///
        /// <returns>
        /// A <see cref="StackableElementOverflowInfo"/> that stores the ID of the <see cref="IStackable"/> whose stack
        /// has been increased, the name of this handler, the overflow amount(0 if no overflow).
        /// </returns>
        ///
        /// <exception cref="KeyNotFoundException">
        /// If the <see cref="TStackable"/> with ID <paramref name="id"/> does not exist.
        /// </exception>
        public StackableElementOverflowInfo IncreaseStack(TID id, int amount)
        {
            if (!_dictionary.TryGetValue(id, out TStackable stackableElement))
            {
                throw new KeyNotFoundException($"StackableElement with ID {id} is not found in {this}.");
            }
            
            int stackBeforeIncrease = stackableElement.Stack;
            
            stackableElement.IncreaseStack(amount, out int overflowAmount);
            
            if (stackBeforeIncrease == 0 && stackableElement.Stack != 0)
            {
                NonZeroCount--;
            }
            else if (stackBeforeIncrease != 0 && stackableElement.Stack == 0)
            {
                NonZeroCount++;
            }
            
            // If overflow happens.
            if (overflowAmount > 0)
            {
                return new StackableElementOverflowInfo(id.ToString(), amount, ToString(), overflowAmount);
            }
            
            return new StackableElementOverflowInfo(id.ToString(), amount, ToString(), 0);
        }
        
        /// <summary>
        /// Decrease the stack number of a <see cref="TStackable"/> with ID <paramref name="id"/> by <paramref name="amount"/>.
        /// </summary>
        /// 
        /// <param name="id">The ID of the <see cref="TStackable"/> to increase stack.</param>
        /// <param name="amount">The amount of stack that will be decreased.</param>
        ///
        /// <returns>
        /// A <see cref="StackableElementOverflowInfo"/> that stores the ID of the <see cref="IStackable"/> whose stack
        /// has been decreased, the name of this handler, the overflow amount(0 if no overflow).
        /// </returns>
        /// 
        /// <exception cref="KeyNotFoundException">
        /// If the <see cref="TStackable"/> with ID <paramref name="id"/> does not exist.
        /// </exception>
        public StackableElementOverflowInfo DecreaseStack(TID id, int amount)
        {
            if (!_dictionary.TryGetValue(id, out TStackable stackableElement))
            {
                throw new KeyNotFoundException($"StackableElement with ID {id} is not found in {this}.");
            }
            
            int stackBeforeDecrease = stackableElement.Stack;
            
            stackableElement.DecreaseStack(amount, out int overflowAmount);
            
            if (stackBeforeDecrease == 0 && stackableElement.Stack != 0)
            {
                NonZeroCount--;
            }
            else if (stackBeforeDecrease != 0 && stackableElement.Stack == 0)
            {
                NonZeroCount++;
            }
            
            // If overflow happens.
            if (overflowAmount > 0)
            {
                return new StackableElementOverflowInfo(id.ToString(), amount, ToString(), overflowAmount);
            }
            
            return new StackableElementOverflowInfo(id.ToString(), amount, ToString(), 0);
        }

        /// <summary>
        /// Check if a <see cref="TStackable"/> with ID <paramref name="id"/> exists in this handler.
        /// </summary>
        /// 
        /// <param name="id">The ID of the <see cref="TStackable"/> to check for existence.</param>
        /// 
        /// <returns>
        /// <c>true</c> if <see cref="TStackable"/> with ID <paramref name="id"/> exists.
        /// Otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TID id)
        {
            return _dictionary.ContainsKey(id);
        }

        /// <summary>
        /// Reset the <see cref="TStackable"/> with ID <paramref name="id"/> in this handler.
        /// </summary>
        /// <param name="id">The ID of the <see cref="TStackable"/> to reset.</param>
        /// 
        /// <exception cref="KeyNotFoundException">
        /// If the <see cref="TStackable"/> with ID <paramref name="id"/> does not exist.
        /// </exception>
        public void ResetSingle(TID id)
        {
            if(!_dictionary.TryGetValue(id, out TStackable stackableElement))
            {
                throw new KeyNotFoundException($"Trying to reset a {typeof(TStackable)}, but stackableElement " +
                                               $"with ID {id} is not found in {this}.");
            }

            int stackBeforeReset = stackableElement.Stack;
            
            stackableElement.Reset();
            
            if (stackBeforeReset == 0 && stackableElement.Stack != 0)
            {
                NonZeroCount--;
            }
            else if (stackBeforeReset != 0 && stackableElement.Stack == 0)
            {
                NonZeroCount++;
            }
        }
        
        /// <summary>
        /// Reset all the <see cref="TStackable"/> in this handler.
        /// </summary>
        public void ResetAll()
        {
            foreach (TStackable stackableElement in _dictionary.Values)
            {
                int stackBeforeReset = stackableElement.Stack;
                
                stackableElement.Reset();
                
                if (stackBeforeReset == 0 && stackableElement.Stack != 0)
                {
                    NonZeroCount--;
                }
                else if (stackBeforeReset != 0 && stackableElement.Stack == 0)
                {
                    NonZeroCount++;
                }
            }
        }
        
        #endregion

        #region IEnumerator

        /// <summary>
        /// Builds an <see cref="IEnumerator{T}"/> from all the <see cref="TStackable"/> in this handler.
        /// </summary>
        /// <returns>All the <see cref="TStackable"/> in this handler.</returns>
        public IEnumerator<TStackable> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}