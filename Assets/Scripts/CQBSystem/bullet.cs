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

        StartCoroutine(DestroyAfterDelay(0.1f)); // �ӳ�0.1�����٣�������Ҫ���Ըı�ʱ��
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void OnCollisionExit(Collision other)
    {
    }
}
