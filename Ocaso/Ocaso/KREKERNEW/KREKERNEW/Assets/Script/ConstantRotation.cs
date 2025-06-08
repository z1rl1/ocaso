using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;  // �������� ��������
    public float pushStrength = 5f;     // ����, � ������� �������� ���������� ���������

    private Rigidbody2D rb;
    private bool isPlayerNear = false;  // ����, ������������, ��� �������� ����� � ���������
    private Rigidbody2D playerRb;       // Rigidbody ���������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ������������� ��� ���� ��� Kinematic, ����� �� ����������� �� ��������
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
            rb.drag = 0;
            rb.angularDrag = 0;
        }
    }

    void Update()
    {
        // ��������� ������� ���������� ������ ��� Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // ���� �������� ����� � ���������, �������� ��� ����
        if (isPlayerNear && playerRb != null)
        {
            // �������� ����������� ���� �� ������ ���� �������� ����������
            Vector2 forceDirection = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // ���������, ��� ��� ��������
        if (other.gameObject.CompareTag("Character"))
        {
            playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            isPlayerNear = true;
        }
    }


    void OnCollisionExit2D(Collision2D other)
    {
        // ����� �������� ������� �� ������� �������� ��������
        if (other.gameObject.CompareTag("Character"))
        {
            isPlayerNear = false;
            playerRb = null;
        }
    }
}