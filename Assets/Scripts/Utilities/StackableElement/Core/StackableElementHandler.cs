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
    public class StackableElementHandler<TID, TStackable> : IEnumerable<KeyValuePair<TID, TStackable>>
        where TID : Enum
        where TStackable : IStackable
    {
        #region Events

        /// <summary>
        /// An event that triggers when a new <see cref="TStackable"/> object is added to the handler.
        /// </summary>
        /// 
        /// /// <remarks>
        /// If did not add successfully, meaning the <see cref="TStackable"/> already exist, this event will not
        /// be triggered.
        /// </remarks>
        public EventHandler<StackableElementStackChangeEventArgs> OnAddStackableElement;

        /// <summary>
        /// An event that triggers when an existing <see cref="TStackable"/> object is deleted to the handler.
        /// </summary>
        ///
        /// <remarks>
        /// If did not delete successfully, meaning the <see cref="TStackable"/> does not exist, this event will not
        /// be triggered.
        /// </remarks>
        public EventHandler<StackableElementStackChangeEventArgs> OnDeleteStackableElement;
        
        /// <summary>
        /// An event that triggers when attempting to increase the <see cref="IStackable.Stack"/> of a <see cref="TStackable"/>
        /// object in this handler.
        /// </summary>
        public EventHandler<StackableElementStackChangeEventArgs> OnIncreaseStack;
        
        /// <summary>
        /// An event that triggers when attempting to decrease the <see cref="IStackable.Stack"/> of a <see cref="TStackable"/>
        /// object in this handler.
        /// </summary>
        public EventHandler<StackableElementStackChangeEventArgs> OnDecreaseStack;
        #endregion
        
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
        /// <see cref="OnAddStackableElement"/> event will be triggered if successfully added a new
        /// <paramref name="stackableElement"/>.
        /// </summary>
        /// 
        /// <param name="id">The ID/name of <see cref="stackableElement"/> that distinguish it among other <see cref="TStackable"/>s.</param>
        /// <param name="stackableElement">The <see cref="TStackable"/> that will be stored in this handler.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// The key <paramref name="id"/> already exists.
        /// </exception>
        public void Add(TID id, [NotNull]TStackable stackableElement)
        {
            if (!_dictionary.TryAdd(id, stackableElement))
            {
                throw new ArgumentException($"An {typeof(TStackable)} with ID {id} already exists in this handler.");
            }

            if (stackableElement.Stack != 0)
            {
                NonZeroCount++;
            }

            StackableElementStackChangeEventArgs eventArgs =
                new StackableElementStackChangeEventArgs(id, 0, stackableElement.Stack);
            OnAddStackableElement?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Completely remove a <see cref="TStackable"/> from this handler. <see cref="OnDeleteStackableElement"/> event
        /// will be triggered if successfully deleted an existing <see cref="TStackable"/> object of ID <paramref name=" id"/>.
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

            if (_dictionary.Remove(id))
            {
                // The possible null reference warning below can be ignored.
                StackableElementStackChangeEventArgs eventArgs =
                    new StackableElementStackChangeEventArgs(id, stackableElement.Stack, 0);
                OnDeleteStackableElement?.Invoke(this, eventArgs);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete all <see cref="TStackable"/>s in this handler.
        /// The <see cref="OnDeleteStackableElement"/> event will be triggered multiple times, depending on how many
        /// <see cref="IStackable"/> objects the handler has.
        /// </summary>
        public void DeleteAll()
        {
            foreach (KeyValuePair<TID,TStackable> kyPair in _dictionary)
            {
                StackableElementStackChangeEventArgs eventArgs =
                    new StackableElementStackChangeEventArgs(kyPair.Key, kyPair.Value.Stack, 0);
                OnDeleteStackableElement?.Invoke(this, eventArgs);

                _dictionary.Remove(kyPair.Key);
            }
            
            NonZeroCount = 0;
        }
        
        /// <summary>
        /// Increase the stack number of a <see cref="TStackable"/> with ID <paramref name="id"/> by <paramref name="amount"/>.
        /// The <see cref="OnIncreaseStack"/> event will be triggered no matter if an overflow occurs.
        /// <see cref="StackableElementStackChangeEventArgs.StackAfterChange"/> will be the actual stack the <see cref="TStackable"/>
        /// object has.
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
        /// <exception cref="ArgumentException">
        /// If the <paramref name="amount"/> is negative.
        /// </exception>
        public StackableElementOverflowInfo IncreaseStack(TID id, int amount)
        {
            if (!_dictionary.TryGetValue(id, out TStackable stackableElement))
            {
                throw new KeyNotFoundException($"StackableElement with ID {id} is not found in {this}.");
            }

            if (amount < 0)
            {
                throw new ArgumentException($"Increasing a negative amount of stack is not allowed. " +
                                            $"{nameof(id)}: {id}, {nameof(amount)}: {amount}.");
            }
            
            int stackBeforeIncrease = stackableElement.Stack;
            
            stackableElement.IncreaseStack(amount, out int overflowAmount);
            
            StackableElementStackChangeEventArgs eventArgs =
                new StackableElementStackChangeEventArgs(id, stackBeforeIncrease, stackableElement.Stack);
            OnIncreaseStack?.Invoke(this, eventArgs);
            
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
        /// The <see cref="OnDecreaseStack"/> event will be triggered no matter if an overflow occurs.
        /// <see cref="StackableElementStackChangeEventArgs.StackAfterChange"/> will be the actual stack the <see cref="TStackable"/>
        /// object has.
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
        /// /// <exception cref="ArgumentException">
        /// If the <paramref name="amount"/> is negative.
        /// </exception>
        public StackableElementOverflowInfo DecreaseStack(TID id, int amount)
        {
            if (!_dictionary.TryGetValue(id, out TStackable stackableElement))
            {
                throw new KeyNotFoundException($"StackableElement with ID {id} is not found in {this}.");
            }
            
            if (amount < 0)
            {
                throw new ArgumentException($"Decreasing a negative amount of stack is not allowed. " +
                                            $"{nameof(id)}: {id}, {nameof(amount)}: {amount}.");
            }
            
            int stackBeforeDecrease = stackableElement.Stack;
            
            stackableElement.DecreaseStack(amount, out int overflowAmount);
            
            StackableElementStackChangeEventArgs eventArgs =
                new StackableElementStackChangeEventArgs(id, stackBeforeDecrease, stackableElement.Stack);
            OnDecreaseStack?.Invoke(this, eventArgs);
            
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
        /// <see cref="OnIncreaseStack"/> or <see cref="OnDecreaseStack"/> will be triggered.
        /// </summary>
        ///
        /// <remarks>
        /// If the <see cref="IStackable.Stack"/> before reset equals to that after the change,
        /// no events will be trigger.
        /// </remarks>
        /// 
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
            
            StackableElementStackChangeEventArgs eventArgs =
                new StackableElementStackChangeEventArgs(id, stackBeforeReset, stackableElement.Stack);
            if (stackableElement.Stack > stackBeforeReset)
            {
                OnIncreaseStack?.Invoke(this, eventArgs);
            }
            else if (stackableElement.Stack < stackBeforeReset)
            {
                OnDecreaseStack?.Invoke(this, eventArgs);
            }
            
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
        ///
        /// <remarks>
        /// If the <see cref="IStackable.Stack"/> before reset equals to that after the change,
        /// no events will be trigger.
        /// </remarks>
        public void ResetAll()
        {
            foreach (KeyValuePair<TID,TStackable> kyPair in _dictionary)
            {
                TID id = kyPair.Key;
                TStackable stackableElement = kyPair.Value;
                
                int stackBeforeReset = stackableElement.Stack;
                
                stackableElement.Reset();
                
                StackableElementStackChangeEventArgs eventArgs =
                    new StackableElementStackChangeEventArgs(id, stackBeforeReset, stackableElement.Stack);
                if (stackableElement.Stack > stackBeforeReset)
                {
                    OnIncreaseStack?.Invoke(this, eventArgs);
                }
                else if (stackableElement.Stack < stackBeforeReset)
                {
                    OnDecreaseStack?.Invoke(this, eventArgs);
                }
                
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
        public IEnumerator<KeyValuePair<TID, TStackable>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}