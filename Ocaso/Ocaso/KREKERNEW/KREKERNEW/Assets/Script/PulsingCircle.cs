using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingCircle : MonoBehaviour
{
    public float targetScale = 0.5f;  // ������, �� �������� ����������� ����
    public float shrinkSpeed = 2f;    // �������� ����������
    public float growSpeed = 2f;      // �������� ����������
    public float shrinkWaitTime = 1f; // ����� �������� ����� ����������
    public float growWaitTime = 1f;   // ����� �������� ����� ����������

    private Vector3 originalScale;
    private float timeCounter = 0f;
    private bool isShrinking = true;
    private bool isWaiting = false;
    private bool isPulsing = false; // ���� ��� ��������� ���������

    void Start()
    {
        // ���������� �������� ������ �������
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isPulsing)
        {
            if (isWaiting)
            {
                // ���� ��������� � ������ ��������, ��������� ������ ��������
                timeCounter += Time.deltaTime;

                // ���������, ������ �� ���������� �������
                if (isShrinking && timeCounter >= shrinkWaitTime)
                {
                    isWaiting = false;
                    timeCounter = 0f;
                    isShrinking = false; // ��������� � ����������
                }
                else if (!isShrinking && timeCounter >= growWaitTime)
                {
                    isWaiting = false;
                    timeCounter = 0f;
                    isShrinking = true; // ��������� � ����������
                }
            }
            else
            {
                if (isShrinking)
                {
                    // ��������� �������
                    float scale = Mathf.Lerp(originalScale.x, targetScale, timeCounter / shrinkSpeed);
                    transform.localScale = new Vector3(scale, scale, scale);

                    // ���� �������� �������� �������, �������� ��������
                    if (timeCounter >= shrinkSpeed)
                    {
                        isWaiting = true;
                        timeCounter = 0f;
                    }
                }
                else
                {
                    // ����������� ������� �������
                    float scale = Mathf.Lerp(targetScale, originalScale.x, timeCounter / growSpeed);
                    transform.localScale = new Vector3(scale, scale, scale);

                    // ���� �������� ��������� �������, �������� ��������
                    if (timeCounter >= growSpeed)
                    {
                        isWaiting = true;
                        timeCounter = 0f;
                    }
                }

                // ����������� ������
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void StartPulsing()
    {
        isPulsing = true;
    }
}
