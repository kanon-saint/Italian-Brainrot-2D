using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletInterval = 0.3f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float bulletMaxDistance = 15f;

    private float attackTimer = 0f;
    private bool isFiring = false;
    private int bulletCount = 1;

    void Start()
    {
        if (weaponData != null)
        {
            UpdateBulletCountBasedOnLevel();
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown && !isFiring)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void UpdateBulletCountBasedOnLevel()
    {
        if (weaponData == null) return;

        // Set bullet count based on level (1 at level 1, up to 5 at level 5)
        bulletCount = Mathf.Clamp(weaponData.level, 1, 5);
    }

    private void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        StartCoroutine(FireBulletsConsecutively(direction, angle));
    }

    private IEnumerator FireBulletsConsecutively(Vector3 direction, float angle)
    {
        isFiring = true;

        for (int i = 0; i < bulletCount; i++)
        {
            if (bulletPrefab != null)
            {
                Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletRotation);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * bulletSpeed;
                }

                bullet.AddComponent<BulletLogic>().Init(transform.position, bulletSpeed, bulletMaxDistance);
            }

            yield return new WaitForSeconds(bulletInterval);
        }

        isFiring = false;
    }

    private class BulletLogic : MonoBehaviour
    {
        private Vector3 spawnPosition;
        private float speed;
        private float maxDistance;

        public void Init(Vector3 origin, float bulletSpeed, float maxDist)
        {
            spawnPosition = origin;
            speed = bulletSpeed;
            maxDistance = maxDist;
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, spawnPosition) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}