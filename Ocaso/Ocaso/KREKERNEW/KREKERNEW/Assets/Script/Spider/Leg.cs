using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public Transform limbSolverTarget;
    public float moveDistance;
    public float moveSpeed = 0.1f; // �������� �������� ����
    public LayerMask groundLayer;

    void Update()
    {
        CheckGround();

        // ��������� ���������� ����� ������� �������� � ������� ��������
        float distance = Vector2.Distance(limbSolverTarget.position, transform.position);

        // ���� ���������� ������ ���������, ��������� � ������� �������
        if (distance > moveDistance)
        {
            // ���������� ����� Lerp ��� �������� �����������
            transform.position = Vector3.Lerp(transform.position, limbSolverTarget.position, moveSpeed * Time.deltaTime);
        }
    }

    public void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector3.down, 5, groundLayer);
        if (hit.collider != null)
        {
            Vector3 point = hit.point;
            point.y += 0.1f;
            limbSolverTarget.position = point;
        }
    }
}