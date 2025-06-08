using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDownController : MonoBehaviour
{
    public float fallSpeed = 2f; // �������� �������
    public float returnSpeed = 2f; // �������� ��������
    public float returnDelay = 2f; // �������� ����� ���������
    public float fallAngle = 45f; // ���� �������

    private Quaternion initialRotation;
    private bool isFalling = false;
    private bool isReturning = false; // ����� ���� ��� ������������ ��������� ��������
    private float targetAngle;

    void Start()
    {
        initialRotation = transform.rotation; // ���������� ��������� �������
    }

    void Update()
    {
        if (isFalling)
        {
            // ��������
            targetAngle += fallSpeed * Time.deltaTime * (180f / Mathf.PI); // ����������� �������� � �������
            transform.rotation = Quaternion.Euler(0, 0, initialRotation.eulerAngles.z - targetAngle);

            // ���������, �������� �� ����� ��������� ����
            if (targetAngle >= fallAngle)
            {
                isFalling = false;
                isReturning = true; // �������� �����������
                StartCoroutine(ReturnToInitialPosition());
            }
        }
    }

    public void StartFalling()
    {
        if (!isFalling && !isReturning) // ������� ������ ���� ����� �� ������ � �� ������������
        {
            isFalling = true;
            targetAngle = 0; // �������� � ����
        }
    }

    private IEnumerator ReturnToInitialPosition()
    {
        yield return new WaitForSeconds(returnDelay);

        while (Mathf.Abs(targetAngle) > 0.01f)
        {
            targetAngle = Mathf.MoveTowards(targetAngle, 0, returnSpeed * Time.deltaTime * (180f / Mathf.PI));
            transform.rotation = Quaternion.Euler(0, 0, initialRotation.eulerAngles.z - targetAngle);
            yield return null;
        }

        transform.rotation = initialRotation; // ���������� �����
        isReturning = false; // ������ ����� ����� ������ � �������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character")) // ���������, ���� ������������� � ����������
        {
            StartFalling(); // ��������� �������
        }
    }
}