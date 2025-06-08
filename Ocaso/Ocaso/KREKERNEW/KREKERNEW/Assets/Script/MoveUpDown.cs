using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 5f; // �������� �������� �������
    public float delay = 3f; // �������� ����� ���������� � ��������

    private bool movingUp = true; // ����, �����������, ��������� �� ������ �����
    private float timer = 0f; // ������ ��� ������������ ��������

    void Update()
    {
        if (movingUp)
        {
            // �������� �����
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // ���� ������ ������ ������ 5, ����������� ����������� �������� � ���������� ������
            if (transform.position.y >= 38f)
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
        if (transform.position.y <= 32f)
        {
            movingUp = true;
            timer = 0f;
        }
    }
}
