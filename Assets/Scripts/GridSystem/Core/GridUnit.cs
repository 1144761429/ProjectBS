using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Core
{
    /// <summary>
    /// Basic unit of a <see cref="GridMap"/>.
    /// </summary>
    ///
    /// <remarks>
    /// A <see cref="GridUnit"/> cannot change it coordinate once created.
    /// </remarks>
    public class GridUnit
    {
        /// <summary>
        /// The coordinate of this <see cref="GridUnit"/> in the <see cref="GridMap"/> space.
        /// </summary>
        public readonly Vector2Int GridCoordinate;
        
        /// <summary>
        /// If there is any <see cref="GridObject"/> placed on this <see cref="GridUnit"/>.
        /// </summary>
        public bool IsOccupied { get; private set; }
        
        /// <summary>
        /// <see cref="GridObject"/>s that are placed on this <see cref="GridUnit"/>.
        /// </summary>
        public readonly Dictionary<Type, List<GridObject>> GridObjectsPlacedOn;

        /// <summary>
        /// Create an unoccupied <see cref="GridUnit"/> with a certain coordinate in the <see cref="GridMap"/> space.
        /// </summary>
        /// 
        /// <param name="gridCoordinate">
        /// The coordinate of this <see cref="GridUnit"/> in the <see cref="GridMap"/> space.
        /// </param>
        public GridUnit(Vector2Int gridCoordinate)
        {
            GridCoordinate = gridCoordinate;
            GridObjectsPlacedOn = new Dictionary<Type, List<GridObject>>();
        }

        /// <summary>
        /// Place the <paramref name="gridObject"/> on this <see cref="GridUnit"/>. 
        /// </summary>
        /// 
        /// <param name="gridObject">The <see cref="GridObject"/> to place.</param>
        /// 
        /// <returns>
        /// True if the <paramref name="gridObject"/> was successfully placed. Otherwise, false.
        /// </returns>
        public bool PlaceGridObject(GridObject gridObject)
        {
            // TODO: check if placing the GridObject on this GridUnit satisfies the dependee requirement of the GridObject.
            
            // TODO: place the GridObject
            
            throw new NotImplementedException();
        }
        
        public void RemoveGridObject(GridObject gridObject)
        {
            throw new NotImplementedException();
        }
    }
}