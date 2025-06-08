using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    public int numberOfClonesAlive = 0;

    // ����� ��� ���������� ���������� ������
    public void AddClone()
    {
        numberOfClonesAlive++;
        Debug.Log("������: " + numberOfClonesAlive);
    }

    // ����� ��� ���������� ���������� ������
    public void RemoveClone()
    {
        numberOfClonesAlive--;
        Debug.Log("������: " + numberOfClonesAlive);

        // ���� ������ ������ ���, �������� ����������� ������
        if (numberOfClonesAlive <= 0)
        {
            // ��� ��� ��� ��������� ���� ��� �������� �� ��������� �������
            Debug.Log("��� ����� �������!");
        }
    }
}
