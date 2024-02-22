using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Core
{
    /// <summary>
    /// Game objects that can be placed on a GridMap.
    /// </summary>
    public abstract class GridObject : MonoBehaviour
    {
        /// <summary>
        /// The grid space of the GridObject will occupy on a <see cref="GridMap"/>. 
        /// </summary>
        public HashSet<Vector2Int> OccupiedArea { get; }
        
        /// <summary>
        /// The position of this GridObject in the GridMap
        /// </summary>
        public Vector2Int GridCoordinate { get; protected set; }

        /// <summary>
        /// The <see cref="GridMap"/> that this GridObject belongs to.
        /// </summary>
        protected GridMap GridMap;
        
        /// <summary>
        /// A dictionary that maps a dependee grid coordinate, respecting to the grid coordinate of this GridObject,
        /// to a list of <see cref="NumberedGridObject"/> that are required to be on that dependee grid coordinate.
        /// </summary>
        public abstract Dictionary<Vector2Int, List<NumberedGridObject>> RequiredDependeeOnCoord { get; }

        public bool Place(GridMap gridMap, Vector2 gridCoordinate)
        {
            throw new NotImplementedException();
        }

        public bool HasRequiredDependees(GridMap gridMap, Vector2 centerGridCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}