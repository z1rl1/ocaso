using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown2lvlBonusAnim : MonoBehaviour
{
    public float speed = 5f; // �������� �������� �������
    public float delay = 3f; // �������� ����� ���������� � ��������

    public float dokuda = 0f;
    public float niz = 0f;

    private bool movingUp = true; // ����, �����������, ��������� �� ������ �����
    private float timer = 0f; // ������ ��� ������������ ��������

    void Update()
    {
        if (movingUp)
        {
            // �������� �����
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // ���� ������ ������ ������ 5, ����������� ����������� �������� � ���������� ������
            if (transform.position.y >= dokuda)
            {
                movingUp = false;
                timer = 0f;
            }
        }
        else
        {
            // ��������� ������
            timer += Time.deltaTime;

            // ���� ������ ����� ��������, �������� �������� ����
            if (timer >= delay)
            {
                MoveDown();
            }
        }
    }

    // ����� ��� �������� ����
    void MoveDown()
    {
        // �������� ����
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // ���� ������ ������ ��������� �������, ����� ����������� ����������� �������� � ���������� ������
        if (transform.position.y <= niz)
        {
            movingUp = true;
            timer = 0f;
        }
    }
}
