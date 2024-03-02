using System;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public GameObject bullet;
        public Transform playerTransform;

        public float speed = 1f;

        public LayerMask shootable;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 clickedWorldPos = Vector3.zero;
                if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, shootable))
                {
                    clickedWorldPos = hitInfo.point;
                }

                Instantiate(bullet, clickedWorldPos, quaternion.identity);
                
                // GameObject go = Instantiate(bullet, playerTransform.position, quaternion.identity);
                //
                // Rigidbody rb = go.GetComponent<Rigidbody>();
                //
                // Vector3 dir = GetShootDirection().normalized;
                //
                // if (dir != Vector3.zero)
                // {
                //     rb.velocity = dir * speed;
                // }
                //
                // Destroy(go, 3);
            }
        }
        
        private Vector3 GetShootDirection()
        {
            
            
            
            return Vector3.zero;
        }
    }
}