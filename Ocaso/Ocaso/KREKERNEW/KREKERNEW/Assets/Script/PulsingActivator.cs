using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingActivator : MonoBehaviour
{
    public GameObject[] pulsatingObjects; // ������ �������� ��� ��������� ���������

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ��� ������ � ����� "Character" ����� � �������
        if (other.CompareTag("Character"))
        {
            // �������� �� ���� ����������� �������� � ���������� ���������
            foreach (GameObject obj in pulsatingObjects)
            {
                PulsingCircle pulsingCircle = obj.GetComponent<PulsingCircle>();
                if (pulsingCircle != null)
                {
                    pulsingCircle.StartPulsing();
                }
            }
        }
    }
}
