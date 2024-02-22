using System;
using System.Linq;
using UnityEngine;

namespace GridSystem.Core
{
    /// <summary>
    /// A controller for how each <see cref="GridUnitVisual"/> should be displayed in <see cref="MainMap"/>.
    /// </summary>
    public class MainMapVisualController
    {
        /// <summary>
        /// The <see cref="GridUnitVisual"/> of this <see cref="MainMapVisualController"/>.
        /// </summary>
        protected readonly GridUnitVisual[,] GridUnitVisualArr;

        
        /// <summary>
        /// Create a <see cref="MainMapVisualController"/> with a 2D array of <see cref="GridUnitVisual"/>s.
        /// </summary>
        /// 
        /// <param name="gridUnitVisualArr">
        /// The <see cref="GridUnitVisual"/>s that this <see cref="MainMapVisualController"/>
        /// can control.
        /// </param>
        public MainMapVisualController(GridUnitVisual[,] gridUnitVisualArr)
        {
            GridUnitVisualArr = gridUnitVisualArr;
        }

        /// <summary>
        /// Show grid coordinate text of a certain grid. If the <paramref name="coord"/> does not
        /// exist, then do nothing.
        /// </summary>
        /// 
        /// <param name="coord">The coordination of the <see cref="GridUnitVisual"/> to show.</param>
        public void ShowSingleGridCoordText(Vector2Int coord)
        {
            try
            {
                GridUnitVisualArr[coord.x, coord.y].ShowCoordText();
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
        
        /// <summary>
        /// Show grid coordinate texts of all the <see cref="GridUnitVisual"/>.
        /// </summary>
        public void ShowAllGridCoordText()
        {
            foreach (GridUnitVisual gridUnitVisual in GridUnitVisualArr)
            {
                gridUnitVisual.ShowCoordText();
            }
        }
        
        /// <summary>
        /// Show grid coordinate texts of all the <see cref="GridUnitVisual"/>, except <see cref="GridUnit"/>
        /// with coordinate listed in <paramref name="exceptions"/>.
        /// </summary>
        /// 
        /// <param name="exceptions">
        /// An array of grid coordinates of <see cref="GridUnitVisual"/>
        /// that will not show grid coordinate texts.
        /// </param>
        public void ShowAllGridCoordTextExcept(Vector2Int[] exceptions)
        {
            foreach (GridUnitVisual gridUnitVisual in GridUnitVisualArr)
            {
                if (exceptions.Contains(gridUnitVisual.GridCoord))
                {
                    continue;
                }
                
                gridUnitVisual.ShowCoordText();
            }
        }
    }
}