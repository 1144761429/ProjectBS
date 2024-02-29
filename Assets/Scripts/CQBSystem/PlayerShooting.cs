using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint; // It's just the player, for now
    public float bulletSpeed = 0.000001f;
    public float gunRadius = 1f;

    // Isomorphism aiming
    public LayerMask raycastLayers; // the enemies or the ground for mousing target
    public float groundOffset = 0.4f; // the height between ground and the player
    public float screenIsoOffset = 25f; // the screen distance from the mousing ground to the isomorphic point

    // bullet spreading
    float spreadAngle = 1f; // initialize
    float minSpreadAngle = 1f;
    float maxSpreadAngle = 12f;
    float spreadIncreasePerShot = 2f; // after every fire
    float spreadRecoveryRate = 10f; // per second

    void Update()
    {
        // Fire1: left mouse buttom or Ctrl, by default
        // TODO: remove Ctrl and assign it to stealth
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }

        // get iso-mousing target
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + Vector3.down * screenIsoOffset);
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayers))
        {
            targetPoint = hit.point;
            //targetPoint.y += groundOffset; // change ground target to iso-aiming direction

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
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position + finalDirection * gunRadius, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = finalDirection * bulletSpeed;
        bullet.transform.forward = rb.velocity;

        if (spreadAngle < maxSpreadAngle)
        {
            spreadAngle += spreadIncreasePerShot;
            spreadAngle = Mathf.Min(maxSpreadAngle, spreadAngle);
        }

        // Destroy-timeout when not colliding
        Destroy(bullet, 5f);


    }
}