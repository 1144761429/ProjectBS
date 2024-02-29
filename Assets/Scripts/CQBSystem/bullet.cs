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
        // 禁用Rigidbody组件，防止进一步的物理运动
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // 设置为Kinematic以停止物理影响
        }

        StartCoroutine(DestroyAfterDelay(0.1f)); // 延迟0.1秒销毁，根据需要可以改变时间
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
