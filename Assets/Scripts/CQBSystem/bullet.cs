using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        GameObject hitObject = collision.collider.gameObject;
        if (hitObject.layer == 7) // is enemy
        { 
            hitObject.GetComponent<EnemyAllInOne>().Damage(1);
        }
        // ����Rigidbody�������ֹ��һ���������˶�
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ����ΪKinematic��ֹͣ����Ӱ��
        }
        GetComponent<MeshRenderer>().enabled = false;
    }


    void OnCollisionExit(Collision other)
    {
    }
}
