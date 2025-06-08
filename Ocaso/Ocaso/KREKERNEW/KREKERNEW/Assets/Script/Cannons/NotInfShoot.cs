using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInfShoot : MonoBehaviour
{
    public GameObject projectilePrefab; // ������ �������
    public Transform shootPoint; // �����, �� ������� ����� �������� �������
    public float shootForce = 400f; // ���� ��������

    public float timeBetweenShots = 1f; // ����� ����� ����������
    public int maxShots = 3; // ������������ ���������� ���������

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
            // �������� �������
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            // ���������� ���� �������� � �������
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(shootPoint.up * shootForce, ForceMode2D.Impulse);
                audioSource.Play();
            }

            // �������� ����� ��������� ���������
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
