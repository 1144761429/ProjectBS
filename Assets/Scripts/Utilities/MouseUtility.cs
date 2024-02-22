using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public static class MouseUtility
    {
        private static Camera Camera => Camera.main;
    
        /// <summary>
        /// Check if mouse is over a game object with certain tag using raycast.
        /// Do not use this to check screen-overlay UI game object.
        /// </summary>
        /// <param name="tag">The tag of the game object to match with.</param>
        /// <returns>
        /// True if the first game object that is hit by the ray is the desired tag.
        /// Otherwise, return false. 
        /// </returns>
        public static bool MouseIsOverGameObjectWithTag(string tag)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue))
            {
                if (hit.transform.gameObject.CompareTag(tag))
                {
                    return true;
                }
            }
        
            return false;
        }
    
        /// <summary>
        /// Check if mouse is over a game object in a certain layer using raycast.
        /// </summary>
        /// <param name="layer">The layer of the game object to be in.</param>
        /// <returns>
        /// True if the first game object that is hit by the ray is in the desired
        /// layer. Otherwise, return false. 
        /// </returns>
        public static bool MouseIsOverLayer(string layer)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(layer))
                {
                    return true;
                }
            }
        
            return false;
        }
    
        /// <summary>
        /// Check if mouse is over a UI game object with certain tag using event system.
        /// </summary>
        /// <param name="tag">The tag of the UI game object to match with.</param>
        /// <returns>
        /// True if the first game object that is hit by the ray is the desired tag.
        /// Otherwise, return false. 
        /// </returns>
        public static bool MouseIsOverUIWithTag(string tag)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

            //return raycastResultList[0].gameObject.CompareTag(tag);
        
            for (int i = 0; i<raycastResultList.Count; i++)
            {
                if (!raycastResultList[i].gameObject.CompareTag(tag))
                {
                    raycastResultList.RemoveAt(i);
                    i--;
                }
            }
        
            return raycastResultList.Count > 0;
        }
    }
}
