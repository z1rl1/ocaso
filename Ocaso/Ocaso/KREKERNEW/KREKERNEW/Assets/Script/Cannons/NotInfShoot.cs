using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInfShoot : MonoBehaviour
{
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform shootPoint; // Точка, из которой будут вылетать снаряды
    public float shootForce = 400f; // Сила выстрела

    public float timeBetweenShots = 1f; // Время между выстрелами
    public int maxShots = 3; // Максимальное количество выстрелов

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void EnableShooting()
    {
        StartCoroutine(ShootProjectilesCoroutine());
    }

    private IEnumerator ShootProjectilesCoroutine()
    {
        for (int i = 0; i < maxShots; i++)
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

            // Ожидание перед следующим выстрелом
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
