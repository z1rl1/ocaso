using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 10f; // ����� ����� �������
    public float damage = 10f;    // ���� ����������� �� ����

    private void Start()
    {
        Destroy(gameObject, lifetime); // ������� ������ ����� ��������� �����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ���������� �� ������ � �����
        if (collision.gameObject.CompareTag("Chain"))
        {
            // �������� HingeJoint2D (��� ������ ���������, ��������� � ������)
            HingeJoint2D hinge = collision.gameObject.GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                // ��������� ��� ��������� ����
                hinge.enabled = false; // ��������� HingeJoint2D
                // ��� ����� ������������ Destroy(collision.gameObject) ���� ������ ��������� ������� ������
            }
        }

    }
}
