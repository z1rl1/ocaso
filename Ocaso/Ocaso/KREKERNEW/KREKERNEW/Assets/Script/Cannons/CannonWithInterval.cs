using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWithInterval : MonoBehaviour
{
    public GameObject projectilePrefab; // ������ �������
    public Transform shootPoint; // ����� �� ������� ����� �������� �������
    public float shootForce = 400f; // ���� ��������

    public float timeBetweenShots = 7f; // ����� ����� ��������� ���������
    public float timeBetweenProjectiles = 1f; // �������� ����� ��������� � �������

    private bool canShoot = false;
    private float lastShotTime;
    private AudioSource audioSource;

    private void Start()
    {
        // ���������� ��������� ��������
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
            // �������� �������
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            // ���������� ���� �������� � �������
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(shootPoint.up * shootForce, ForceMode2D.Impulse);
                audioSource.Play();
            }

            // �������� ����� ��������� �������� � �������
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }
}
