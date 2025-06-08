using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWithInterval : MonoBehaviour
{
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform shootPoint; // Точка из которой будут вылетать снаряды
    public float shootForce = 400f; // Сила выстрела

    public float timeBetweenShots = 7f; // Время между очередями выстрелов
    public float timeBetweenProjectiles = 1f; // Интервал между снарядами в очереди

    private bool canShoot = false;
    private float lastShotTime;
    private AudioSource audioSource;

    private void Start()
    {
        // Сбрасываем состояние стрельбы
        canShoot = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canShoot && Time.time >= lastShotTime + timeBetweenShots)
        {
            lastShotTime = Time.time;
            StartCoroutine(ShootProjectilesCoroutine());
        }
    }

    public void EnableShooting()
    {
        canShoot = true;
    }

    public void DisableShooting()
    {
        canShoot = false;
    }

    private System.Collections.IEnumerator ShootProjectilesCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            // Создание снаряда
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            // Применение силы выстрела к снаряду
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(shootPoint.up * shootForce, ForceMode2D.Impulse);
                audioSource.Play();
            }

            // Ожидание перед следующим снарядом в очереди
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }
}
