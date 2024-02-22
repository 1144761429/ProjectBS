using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace GridSystem.Core
{
    public class GridUnitVisual : MonoBehaviour
    {
        protected readonly Color ColorDefault = new Color32(138, 215, 225,255);
        protected readonly Color ColorConstructable = new Color32(0, 230, 77,255);
        protected readonly Color ColorUnconstructable = new Color32(255, 71, 26,255);
        
        /// <summary>
        /// The coordinate text of this <see cref="GridUnitVisual"/>.
        /// </summary>
        [SerializeField] private TMP_Text coordinateText;
        
        // TODO: add comment.
        [SerializeField] private MeshRenderer indicator;

        /// <summary>
        /// The coordinate of the <see cref="GridUnit"/> this <see cref="GridUnitVisual"/> links to.
        /// </summary>
        public Vector2Int GridCoord => _gridUnit.GridCoordinate;
        
        /// <summary>
        /// The <see cref="GridUnit"/> this <see cref="GridUnitVisual"/> links to.
        /// </summary>
        private GridUnit _gridUnit;
        
        
        private void Awake()
        {
            Material materialInstance = new Material(indicator.material); //TODO: maybe dont need to create a new one?
            indicator.material = materialInstance;
        }

        /// <summary>
        /// Set the corresponding <see cref="GridUnit"/>.
        /// This <see cref="GridUnitVisual"/> will now display the status of <paramref name="gridUnit"/>.
        /// </summary>
        ///
        /// <remarks>
        /// <see cref="coordinateText"/> will be set/updated after linking.
        /// </remarks>
        /// 
        /// <param name="gridUnit">The <see cref="GridUnit"/> to link to.</param>
        public void LinkToGridUnit(GridUnit gridUnit)
        {
            _gridUnit = gridUnit;

            Vector2Int coordinate = gridUnit.GridCoordinate;
            coordinateText.text = $"X{coordinate.x} Y{coordinate.y}";
        }
        
        /// <summary>
        /// Display both the <see cref="indicator"/> and the <see cref="coordinateText"/>.
        /// </summary>
        public void Show()
        {
            ShowIndicator();
            ShowCoordText();
        }
        
        /// <summary>
        /// Hide both the <see cref="indicator"/> and the <see cref="coordinateText"/>.
        /// </summary>
        public void Hide()
        {
            HideIndicator();
            HideCoordText();
        }
        
        /// <summary>
        /// Display the <see cref="indicator"/>.
        /// </summary>
        public void ShowIndicator()
        {
            indicator.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Hide the <see cref="indicator"/>.
        /// </summary>
        public void HideIndicator()
        {
            indicator.gameObject.SetActive(false);
        }

        /// <summary>
        /// Display the <see cref="coordinateText"/>.
        /// </summary>
        public void ShowCoordText()
        {
            coordinateText.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Hide the <see cref="coordinateText"/>.
        /// </summary>
        public void HideCoordText()
        {
            coordinateText.gameObject.SetActive(false);
        }
        
        #region Change Indicator

        
        /// <summary>
        /// Set the material of the indicator to another according to <paramref name="indicatorType"/>.
        /// This method will not show/enable the indicator but just change the material.
        /// </summary>
        /// <param name="indicatorType">The new material that the indicator is going to switch to.</param>
        public void ChangeIndicator(EGridUnitIndicator indicatorType)
        {
            switch (indicatorType)
            {
                case EGridUnitIndicator.Default:
                    indicator.material.color = ColorDefault;
                    break;
                case EGridUnitIndicator.Constructable:
                    indicator.material.color = ColorConstructable;
                    break;
                case EGridUnitIndicator.Unconstructable:
                    indicator.material.color = ColorUnconstructable;
                    break;
            }
        }
        
        
        /// <summary>
        /// A wrapper method of ChangeIndicator(EGridUnitIndicator.Default).
        /// </summary>
        public void ChangeIndicatorToDefault()
        {
            ChangeIndicator(EGridUnitIndicator.Default);
        }
        
        
        /// <summary>
        /// A wrapper method of ChangeIndicator(EGridUnitIndicator.Constructable).
        /// </summary>
        public void ChangeIndicatorToConstructable()
        {
            ChangeIndicator(EGridUnitIndicator.Constructable);
        }
        
        
        /// <summary>
        /// A wrapper method of ChangeIndicator(EGridUnitIndicator.Unconstructable).
        /// </summary>
        public void ChangeIndicatorToUnconstructable()
        {
            ChangeIndicator(EGridUnitIndicator.Unconstructable);
        }
        
        
        #endregion
        
        
        // TODO: in future, set the positionText in Awake() by converting world position to grid position.
        public void SetPositionText(string text)
        {
            coordinateText.text = text;
        }
    }
}