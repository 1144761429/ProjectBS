using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;


public class PlayerAllInOne : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 10f;
    public GameObject model; 
    public GameObject playerCollider; 
    public GameObject frontCollider;
    private float moveSpeed = 0f;

    private Rigidbody rb;
    private Transform bulletSpawnPoint;
    private Animator animator;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    
    public GameObject lightPrefab;
    public GameObject bulletPrefab;
    private new GameObject light;
    private ObjectPool<GameObject> BulletPool;

    public Transform player;
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

    private float recentShot = 0f;  // for animator to tell when to stop shooting animation
    public float shotPeriod = 1f;  // how long till the next shot

    private void Awake()
    {
        BulletPool = new ObjectPool<GameObject>(OnCreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, 16, 100);
        light = Instantiate(lightPrefab);
        light.SetActive(false);
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
        animator = model.GetComponent<Animator>();
        rb = playerCollider.GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        bulletSpawnPoint = frontCollider.transform;
    }
    private void Update()
    {
        if (Input.GetButton("Ctrl"))
        {
            animator.SetBool("Aiming", true);
            moveVelocity = Vector3.zero;
            animator.SetBool("Moving", false);

            // get iso-mousing target
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, 100, raycastLayers))
            {
                targetPoint = hit.point;

                Vector3 direction = targetPoint - player.position;
                direction.y = 0;
                player.forward = direction;
                rb.transform.forward = direction;
            }
            if (Input.GetButton("Fire1"))
            {
                if (recentShot < 0)
                    FireBullet();
            }
        }
        else
        {
            animator.SetBool("Aiming", false);
            // Get input from the horizontal and vertical axis (WASD or arrow keys by default)
            float horiInput = Input.GetAxis("Horizontal");
            float vertiInput = Input.GetAxis("Vertical");

            moveInput = new Vector3(horiInput, 0f, vertiInput);
            moveVelocity = moveInput.normalized * moveSpeed;

            if (horiInput != 0 || vertiInput != 0)
            {
                animator.SetBool("Moving", true);
                rb.transform.forward = moveInput;
            }
            else animator.SetBool("Moving", false);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = runSpeed;
                animator.SetBool("Running", true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = walkSpeed;
                animator.SetBool("Running", false);
            }
        }
    }

    private void FixedUpdate()
    {
        // spread control
        if (spreadAngle > minSpreadAngle)
        {
            spreadAngle -= spreadRecoveryRate * Time.fixedDeltaTime;
            spreadAngle = Mathf.Max(minSpreadAngle, spreadAngle);
        }
        if (recentShot > -0.05f)
        {
            recentShot -= Time.fixedDeltaTime;
            //recentShot = Mathf.Max(-0.05f, recentShot);
            if (recentShot < 0.75f) { animator.SetBool("Firing", false); }
        }
        

        // Apply the movement to the Rigidbody
        rb.AddForce(moveVelocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }



    void FireBullet()
    {
        animator.SetBool("Firing", true);
        recentShot = shotPeriod;

        //TODO: ShotGun type check
        for (int i = 0; i < 1; i++)
        {

            // Random spread inside a unit cone
            Vector3 randomDirection = Random.insideUnitCircle * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
            randomDirection.z = 1f;
            randomDirection = randomDirection.normalized;

            Vector3 finalDirection = Quaternion.LookRotation(player.forward.normalized) * randomDirection;



            // create the bullet instance
            GameObject bullet = BulletPool.Get();
            light.transform.position = bulletSpawnPoint.position + finalDirection * gunRadius;
            light.SetActive(true);
            bullet.transform.position = light.transform.position;
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
            StartCoroutine(DisableLight(light));

        }

    }

    // destroy after 1s
    private IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(1);
        BulletPool.Release(bullet);
    }
    private IEnumerator DisableLight(GameObject light)
    {
        yield return new WaitForSeconds(0.1f);
        light.SetActive(false);
    }
}
