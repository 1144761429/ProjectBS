using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // ����Rigidbody�������ֹ��һ���������˶�
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ����ΪKinematic��ֹͣ����Ӱ��
        }
    }


    void OnCollisionExit(Collision other)
    {
    }
}
