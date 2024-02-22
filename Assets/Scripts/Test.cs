using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public GameObject obj;

        private void Start()
        {
            //GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        }
    }
}