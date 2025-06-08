using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHeadFollow : MonoBehaviour
{
    public Transform character; // ������ �� ������ ���������
    public float smoothness = 5f; // ��������� ���������� ������ �� ����������

    private void Update()
    {
        if (character != null)
        {
            // �������� ����������� �� ������ ����� � ���������
            Vector2 direction = character.position - transform.position;

            // ��������� ���� ����� ������� ����� � ����������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ������� ����� ������ �������� �� ��� Z
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // ������ ������������ ������ ����� � ������ �����������
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
        }
    }
}


