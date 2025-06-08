using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDynObjLvl3DieZone : MonoBehaviour
{
    public string targetTag = "DieZone"; // ��� ��������, ������� ����� ����������
    public float dropDistance = 10f; // ����������, �� ������� ����� �������� �������
    public float dropDuration = 5f; // �����, �� ������� ������� ������ ����������
    public float raiseDuration = 5f; // �����, �� ������� ������� ������ ���������
    public float waitTime = 1f; // ����� �������� ����� ��������

    private bool isActivated = false; // ���� ��������� ��������

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && !isActivated) // ���������, ��� ��� �������� � ������� ��� �� �����������
        {
            isActivated = true; // ������������� ����, ����� ������������� ��������� ���������
            StartCoroutine(HandleObjects()); // ��������� �������� ��� ��������� ��������
        }
    }

    private IEnumerator HandleObjects()
    {
        GameObject[] objectsToHandle = GameObject.FindGameObjectsWithTag(targetTag); // ������� ��� ������� � �������� �����

        // ��������� �������� ��� ������� �������
        foreach (GameObject obj in objectsToHandle)
        {
            StartCoroutine(DropAndRaiseObject(obj.transform));
        }

        yield return null; // ��������� ��������
    }

    private IEnumerator DropAndRaiseObject(Transform objTransform)
    {
        while (true) // ����������� ����
        {
            Vector3 startPos = objTransform.position;
            Vector3 dropPos = new Vector3(startPos.x, startPos.y - dropDistance, startPos.z);
            float elapsedTime = 0f;

            // ������ �������� ������
            while (elapsedTime < dropDuration)
            {
                objTransform.position = Vector3.Lerp(startPos, dropPos, elapsedTime / dropDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // ���� �� ���������� �����
            }

            objTransform.position = dropPos;

            // ���� ����� ��������
            yield return new WaitForSeconds(waitTime);

            // ������ ��������� ������ �������
            elapsedTime = 0f; // ����� �������
            while (elapsedTime < raiseDuration)
            {
                objTransform.position = Vector3.Lerp(dropPos, startPos, elapsedTime / raiseDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // ���� �� ���������� �����
            }

            objTransform.position = startPos;

            // ���� ����� ������� ���������� �����
            yield return new WaitForSeconds(waitTime);
        }
    }
}
        
