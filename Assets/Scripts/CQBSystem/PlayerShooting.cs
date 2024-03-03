using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    private ObjectPool<GameObject> BulletPool;

    public Transform bulletSpawnPoint; // It's just the player, for now
    public float bulletSpeed = 100f;
    public float gunRadius = 1f;

    // Isomorphism aiming
    public LayerMask raycastLayers; // the enemies or the ground for mousing target

    // bullet spreading
    public float spreadAngle = 1f; // initialize
    public float minSpreadAngle = 1f;
    public float maxSpreadAngle = 12f;
    public float spreadIncreasePerShot = 2f; // after every fire
    public float spreadRecoveryRate = 10f; // per second

    private void Awake()
    {
        BulletPool = new ObjectPool<GameObject>(OnCreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, 16, 100);
    }

    private GameObject OnCreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        return bullet;
    }

    private void OnGetBullet(GameObject bullet)
    {
    }

    private void OnReleaseBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
    private void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    private void Start()
    {

    }

    private void Update()
    {
        // Fire1: left mouse buttom or Ctrl, by default
        // TODO: remove Ctrl and assign it to stealth
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }

        // get iso-mousing target
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 100, raycastLayers))
        {
            targetPoint = hit.point;

            Vector3 direction = targetPoint - bulletSpawnPoint.position;
            direction.y = bulletSpawnPoint.forward.y;
            bulletSpawnPoint.forward = direction;
        }
    }

    void FixedUpdate()
    {
        // spread control
        if (spreadAngle > 0)
        {
            spreadAngle -= spreadRecoveryRate * Time.fixedDeltaTime;
            spreadAngle = Mathf.Max(minSpreadAngle, spreadAngle);
        }
    }
    void FireBullet()
    {
        // Random spread inside a unit cone
        Vector3 randomDirection = Random.insideUnitCircle * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
        randomDirection.z = 1f;
        randomDirection = randomDirection.normalized;

        Vector3 finalDirection = Quaternion.LookRotation(bulletSpawnPoint.forward.normalized) * randomDirection;



        // create the bullet instance
        GameObject bullet = BulletPool.Get();
        bullet.transform.position = bulletSpawnPoint.position + finalDirection * gunRadius;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().isKinematic = false;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = finalDirection * bulletSpeed;
        bullet.transform.forward = rb.velocity;

        if (spreadAngle < maxSpreadAngle)
        {
            spreadAngle += spreadIncreasePerShot;
            spreadAngle = Mathf.Min(maxSpreadAngle, spreadAngle);
        }

        // timeout-destroy
        StartCoroutine(DestroyBullet(bullet));

    }

    // destroy after 1s
    private IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(1);
        BulletPool.Release(bullet);
    }
}