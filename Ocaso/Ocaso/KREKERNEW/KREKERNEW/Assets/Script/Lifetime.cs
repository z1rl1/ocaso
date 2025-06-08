using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 3.0f; // ����� ����� ������� � ��������

    void Start()
    {
        Invoke("DestroyObject", lifetime); // ��������� ������� DestroyObject ����� lifetime ������
    }

    void DestroyObject()
    {
        // ���������� ������
        Destroy(gameObject);
    }
}