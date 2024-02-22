using UnityEngine;
using UnityEngine.Serialization;

namespace GridSystem.Core
{
    /// <summary>
    /// A map based on grid system.
    /// </summary>
    public abstract class GridMap : MonoBehaviour
    {
        /// <summary>
        /// A vector2 that represents the length and width of a GridMap.
        /// Note: The y of this vector2 is the z-axis in the scene.
        /// </summary>
        [SerializeField] protected Vector2Int dimension;

        /// <summary>
        /// The size of a grid in this GridMap.
        /// </summary>
        [SerializeField] private int gridSize;
        
        /// <summary>
        /// The center offset of the visual of a grid. X of this vector2 is the x-axis offset,
        /// and y of this vector2 is the z-axis offset.
        /// 
        /// Since the pivot of each grid in a grid map is the bottom left corner, it would be
        /// wrong if we instantiate the visual at pivot. Therefore, we need an offset to place
        /// the visual at the correct position.
        /// Usually, this would be half of the size of each grid.
        /// </summary>
        [SerializeField] private Vector2 gridVisualOffsetFromCenter;
        
        /// <summary>
        /// The GridUnitVisualPrefab that we are going to instantiate.
        /// </summary>
        [SerializeField] private GridUnitVisual gridUnitVisualPrefab;
        
        /// <summary>
        /// The transform that is as the parent/root object of all the GridObject on this GridMap.
        /// </summary>
        [SerializeField] private Transform gridObjectRoot;

        /// <summary>
        /// The transform that is the parent/root object of all the GridUnitVisual of this GridMap.
        /// </summary>
        [SerializeField] public Transform gridUnitVisualRoot;
        
        /// <summary>
        /// All the <see cref="GridUnit"/>s in this <see cref="GridMap"/>.
        /// </summary>
        protected GridUnit[,] _gridUnitArr;

        /// <summary>
        /// All the <see cref="GridUnitVisual"/>s of this <see cref="GridMap"/>.
        /// Each <see cref="GridUnitVisual"/> corresponds to a <see cref="GridUnit"/>. 
        /// </summary>
        protected GridUnitVisual[,] _gridUnitVisualsArr;
        
        
        protected virtual void Awake()
        {
            _gridUnitArr = new GridUnit[dimension.x, dimension.y];
            _gridUnitVisualsArr = new GridUnitVisual[dimension.x, dimension.y];
            
            for (int row = 0; row < dimension.x; row++)
            {
                for (int column = 0; column < dimension.y; column++)
                {
                    // Instantiate the GridUnitVisual to its corresponding position.
                    GridUnitVisual gridUnitVisual = Instantiate(gridUnitVisualPrefab,
                        transform.position + new Vector3(row * gridSize + gridVisualOffsetFromCenter.x, 0f,
                            column * gridSize + gridVisualOffsetFromCenter.y),
                        gridUnitVisualPrefab.transform.rotation);
                    gridUnitVisual.name = $"Visual X{row} Y{column}";
                    
                    // We separate the parent setting from Instantiate() because if we do parent setting in the
                    // Instantiate() method, the GridUnitVisual will have the scale of the parent.
                    gridUnitVisual.transform.SetParent(gridUnitVisualRoot);
                    
                    // Store the gridUnitVisual to the GridUnitVisualArr 2D array, and set the grid position text.
                    _gridUnitVisualsArr[row, column] = gridUnitVisual;
                    gridUnitVisual.ChangeIndicatorToDefault();
                    gridUnitVisual.SetPositionText($"X{row} Y{column}");
                    
                    // Create and store the GridUnit to the 2D array of that.
                    GridUnit gridUnit = new GridUnit(new Vector2Int(row, column));
                    _gridUnitArr[row, column] = gridUnit;

                    gridUnitVisual.LinkToGridUnit(gridUnit);
                }
            }
        }
        
        /// <summary>
        /// Use two ints to get a <see cref="GridUnit"/> at grid coordinate (<paramref name="x"/>, <paramref name="y"/>).
        /// </summary>
        /// <param name="x">The X axis position of the <see cref="GridUnit"/> to retrieve.</param>
        /// <param name="y">The Y axis position of the <see cref="GridUnit"/> to retrieve.</param>
        public GridUnit this[int x, int y] => _gridUnitArr[x, y];
        
        /// <summary>
        /// Use a <see cref="Vector2Int"/> to get a <see cref="GridUnit"/> at grid coordinate <paramref name="coordinate"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate of the <see cref="GridUnit"/> to retrieve.</param>
        public GridUnit this[Vector2Int coordinate] => _gridUnitArr[coordinate.x, coordinate.y];
        
        
        
        
        
        
        
        
        
        
        
        
        /*
        
        /// <summary>
        /// Show all the GridUnitVisual.
        /// </summary>
        public void ShowAllGridUnitVisual()
        {
            gridUnitVisualRoot.gameObject.SetActive(true);
        }
        
        
        /// <summary>
        /// Hide all the GridUnitVisual.
        /// </summary>
        public void HideAllGridUnitVisual()
        {
            gridUnitVisualRoot.gameObject.SetActive(false);
        }
        
        
        /// <summary>
        /// Show the grid position info for this GridMap.
        /// </summary>
        public void ShowAllGridPositionText()
        {
            foreach (var visual in GridUnitVisualArr)
            {
                visual.ShowPositionText();
            }
        }
        
        
        /// <summary>
        /// Hide the grid position info for this GridMap.
        /// </summary>
        public void HideAllGridPositionText()
        {
            foreach (var visual in GridUnitVisualArr)
            {
                visual.HidePositionText();
            }
        }
        
        
        /// <summary>
        /// Show the visual of all GridUnitIndicator of this GridMap.
        /// </summary>
        public void ShowAllGridUnitIndicator()
        {
            foreach (var visual in GridUnitVisualArr)
            {
                visual.ShowIndicator();
            }
        }
        
        
        /// <summary>
        /// Hide the visual of all GridUnitIndicator of this GridMap.
        /// </summary>
        public void HideAllGridUnitIndicator()
        {
            foreach (var visual in GridUnitVisualArr)
            {
                visual.HideIndicator();
            }
        }
        
        
        /// <summary>
        /// Change the indicator visual of all the grid in this GridMap to <paramref name="indicatorType"/>.
        /// This method only change the material of the indicator and does not show the visual. Therefore,
        /// to display indicator after calling this method while indicators are not shown, please call
        /// ShowAllGridUnitVisual().
        /// </summary>
        /// <param name="indicatorType">The indicator visual type to change to.</param>
        public void ChangeAllIndicator(EGridUnitIndicator indicatorType)
        {
            foreach (var visual in GridUnitVisualArr)
            {
                visual.ChangeIndicator(indicatorType);
            }
        }
*/
        
        #region Coordinate Conversion Methods

        
        /// <summary>
        /// Convert the mouse position in the screen space coordinate to grid space coordinate.
        /// </summary>
        /// <param name="gridCoordinate">The grid space coordinate transformed from mouse coordinate.
        /// This value will be Vector2Int.zero if mouse is not pointing at a grid map.</param>
        /// <returns>True if the mouse is pointing at the grid map when calling this method. Otherwise, false.</returns>
        public bool MouseToGridCoordinate(out Vector2Int gridCoordinate)
        {
            // When Using a 3D Perspective, Camera must set the Z value of Input.MousePosition to a
            // positive value (such as the Camera’s Near Clip Plane) before passing it into ScreenToWorldPoint.
            // If don’t, no movement will be detected.
            Vector3 modifiedScreenSpace = Input.mousePosition;
            modifiedScreenSpace.z = Camera.main.nearClipPlane;

            Ray ray = Camera.main.ScreenPointToRay(modifiedScreenSpace);

            // The layer to detect raycast.
            int layerMasks = LayerMask.GetMask("Grid Map");

            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, layerMasks))
            {
                Vector3 worldCoordinate = hit.point;
                gridCoordinate = WorldToGridCoordinate(worldCoordinate);
                return true;
            }

            gridCoordinate = Vector2Int.zero;
            return false;
        }

        /// <summary>
        /// Convert a world space coordinate to grid space coordinate.
        /// </summary>
        /// <param name="worldCoordinate">The world space coordinate to change to grid space coordinate.</param>
        /// <returns>The converted grid coordinate.</returns>
        public Vector2Int WorldToGridCoordinate(Vector3 worldCoordinate)
        {
            int gridCoordinateX = (int)(worldCoordinate.x / gridSize);
            int gridCoordinateY = (int)(worldCoordinate.z / gridSize);
            
            return new Vector2Int(gridCoordinateX, gridCoordinateY);
        }

        /// <summary>
        /// Convert a grid space coordinate to world space coordinate.
        /// </summary>
        /// <param name="gridCoordinate">The grid space coordinate to change to world space coordinate.</param>
        /// <returns>The converted world coordinate.</returns>
        public Vector3 GridToWorldCoordinate(Vector2Int gridCoordinate)
        {
            float worldCoordinateX = gridCoordinate.x * gridSize + gridVisualOffsetFromCenter.x;
            float worldCoordinateZ = gridCoordinate.y * gridSize + gridVisualOffsetFromCenter.y;

            return new Vector3(worldCoordinateX, 0, worldCoordinateZ);
        }
        

        #endregion
        
        
    }
}
