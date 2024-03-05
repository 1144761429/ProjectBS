using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    public bool collide;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        collide = true;
    }

    void OnCollisionExit(Collision other)
    {
        collide = false;
    }
}
