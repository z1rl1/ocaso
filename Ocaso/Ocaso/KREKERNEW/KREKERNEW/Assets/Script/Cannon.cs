using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject character; // ������ �� ��� ������������� ���������
    public Transform firePoint; // ����� ��������
    public float shootForce = 100f; // ���� ��������
    public float timedelay = 5f;

    private void Start()
    {
        // ��������� ��������, ������� ����� ������������ ����� 5 ������
        StartCoroutine(ShootAfterDelay(timedelay));
    }

    private IEnumerator ShootAfterDelay(float delay)
    {
        // ���� �������� ���������� ������
        yield return new WaitForSeconds(delay);

        // ���������, ��� �������� ����������
        if (character != null)
        {
            // ���������� ��������� � ����� ��������
            character.transform.position = firePoint.position;

            // �������� Rigidbody2D ���������� ��������� ��� ���������� ����
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ��������� ���� ��� �������� ������
                rb.AddForce(firePoint.right * shootForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("�������� �� ��������!");
        }
    }
}