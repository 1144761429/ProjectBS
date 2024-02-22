using System;
using System.Collections;
using GridSystem.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using Object = UnityEngine.Object;

namespace GridSystem
{
    // TODO: add comment
    public enum EMainMapStatus
    {
        Default = 0,
        ConstructionMode = 1
    }

    public class MainMap : GridMap
    {
        public GridObject ObjToPCandidate1;
        public GridObject ObjToPCandidate2;
        
        /// <summary>
        /// The singleton instance of <see cref="MainMap"/>.
        /// </summary>
        public static MainMap Instance { get; private set; }


        /// <summary>
        /// The <see cref="GridObject"/> that is placed when the user attempts to build in construction mode.
        /// </summary>
        private GridObject _gridObjectToPlace;

        /// <summary>
        /// The current <see cref="EMainMapStatus"/> of the <see cref="MainMap"/>.
        /// </summary>
        public EMainMapStatus Status { get; private set; }

        /// <summary>
        /// A <see cref="MainMapVisualController"/> for <see cref="MainMap"/>.
        /// </summary>
        public MainMapVisualController VisualController { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            VisualController = new MainMapVisualController(_gridUnitVisualsArr);
            
            // Initialize singleton
            if (Instance != null)
            {
                Destroy(this);
                throw new Exception(
                    $"Multiple instance of MainMap exists. Extra one on {gameObject.name} has been destroyed.");
            }
            Instance = this;
        }

        private void Update()
        {
            switch (Status)
            {
                case EMainMapStatus.Default:
                    break;
                case EMainMapStatus.ConstructionMode:
                    ConstructionModeLogic();
                    break;
            }
        }

        
        #region Mode Control
        
        /// <summary>
        /// Enter construction mode of the main map.
        /// </summary>
        public void EnterConstructionMode()
        {
            Status = EMainMapStatus.ConstructionMode;
            
            // Show the constructable status of all grids.
            
            // Change all indicator to default.
            
            // Show buttons of selecting object to place TODO separate UI from logic
        }

        /// <summary>
        /// Exit the build mde of the main map.
        /// </summary>
        public void ExitBuildMode()
        {
            //HideAllGridUnitIndicator();

            Status = EMainMapStatus.Default;
        }

        private void ConstructionModeLogic()
        {
            /*
            TODO: case 1: No GridObject picked -> show default. give a toggle to see occupied and unoccupied
            
            TODO: case 2: GridObject picked
            
            TODO: case 3: GridObject placed but not confirmed -> show valid or invalid place coordinate
            If the place is valid for placement, change the GridUnits to "green?"
            If the place is invalid for placement, change the invalid GridUnits to "red?
            */
            
            if (Input.GetMouseButtonDown(0)
                && !EventSystem.current.IsPointerOverGameObject()
                && MouseUtility.MouseIsOverLayer("Grid Map")
                && _gridObjectToPlace != null)
            {
                MouseToGridCoordinate(out Vector2Int gridCoordinate);

                if (_gridUnitArr[gridCoordinate.x, gridCoordinate.y].PlaceGridObject(_gridObjectToPlace))
                {
                    Debug.Log("Placed");
                   // RefreshVisualObjectToBuildSelected(_gridObjectToPlace);
                }
            }
        }

        #endregion

/*
        #region Indicator Related
        
        private void RefreshIndicatorBuildStandBy()
        {
            if (Status != EMainMapStatus.BuildModeStandBy)
            {
                Debug.LogError("Trying to call RefreshIndicatorWhenBuildStandBy when the status is not BuildStandBy");
            }
            
            for (int row = 0; row < Dimension.x; row++)
            {
                for (int column = 0; column < Dimension.y; column++)
                {
                    if (_gridUnitArr[row, column].IsOccupied)
                    {
                        GridUnitVisualArr[row, column].ChangeIndicatorToUnconstructable();
                    }
                }
            }
        }
        
        
        private void RefreshVisualObjectToBuildSelected(GridObject objectToBuild)
        {
            if (Status != EMainMapStatus.BuildModeObjectToBuildSelected)
            {
                Debug.LogError("Trying to call RefreshIndicatorWhenBuildStandBy when the status is not BuildStandBy");
            }
            
            for (int row = 0; row < Dimension.x; row++)
            {
                for (int column = 0; column < Dimension.y; column++)
                {
                    GridUnitVisualArr[row, column].ChangeIndicatorToUnconstructable();
                    
                    if (objectToBuild.RequiresDependee)
                    {
                        // If the GridUnit is occupied(something is placed on it) and
                        // the dependee are placed on this GridUnit already,
                        // then change the visual to constructable. Otherwise, unconstructable.
                        if (_gridUnitArr[row, column].IsOccupied &&
                            _gridUnitArr[row, column].DenpendeeSatisfied(objectToBuild))
                        {
                            GridUnitVisualArr[row,column].ChangeIndicatorToConstructable();
                        }
                        else
                        {
                            GridUnitVisualArr[row,column].ChangeIndicatorToUnconstructable();
                        }
                    }
                    else
                    {
                        // If the GridUnit is occupied(something is placed on it), then
                        // change the visual to unconstructable. Otherwise, constructable.
                        if (_gridUnitArr[row, column].IsOccupied)
                        {
                            GridUnitVisualArr[row,column].ChangeIndicatorToUnconstructable();
                        }
                        else
                        {
                            GridUnitVisualArr[row,column].ChangeIndicatorToConstructable();
                        }
                    }
                }
            }
        }

        #endregion
        
        
        #region Test purpose 

        
        public void SwitchTo1()
        {
            _gridObjectToPlace = ObjToPCandidate1;
            Status = EMainMapStatus.BuildModeObjectToBuildSelected;
            RefreshVisualObjectToBuildSelected(ObjToPCandidate1);
        }
        
        public void SwitchTo2()
        {
            _gridObjectToPlace = ObjToPCandidate2;
            Status = EMainMapStatus.BuildModeObjectToBuildSelected;
            RefreshVisualObjectToBuildSelected(ObjToPCandidate2);
        }
        

        #endregion
        */
    }
}