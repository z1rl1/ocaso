using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPosition : MonoBehaviour
{
    public Vector3 initialPosition;
    public bool isPicked = false; // �������� ��� ����
    private void Start()
    {
        initialPosition = transform.position; // ��������� �������������� ������� ��� �������� ������
    }
}