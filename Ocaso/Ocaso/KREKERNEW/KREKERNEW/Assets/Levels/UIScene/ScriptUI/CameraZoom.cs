using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.TextCore.Text;

public class CameraZoom : MonoBehaviour
{
    public Camera camera; // ������ �� ������
    public PixelPerfectCamera ppc; // ������ �� ��������� PixelPerfectCamera
    public Vector3 targetPosition = new Vector3(-389f, 48f, 0f); // ������� ������� ������
    public Vector2 targetResolution = new Vector2(1920f, 1080f); // ������� ����������
    public float duration = 2f; // ����� �������� ��������� ���������� � �������
    public float delayTime = 1f; // ����� �������� ����� ������� ��������

    private Vector2 initialResolution; // ��������� ����������
    private Vector3 initialPosition; // ��������� ������� ������

    void Start()
    {
        initialResolution = new Vector2(ppc.refResolutionX, ppc.refResolutionY);
        initialPosition = camera.transform.position;

        StartCoroutine(CameraTransition());
    }

    IEnumerator CameraTransition()
    {
        // �������� ����� ������� ��������
        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;

        // ������� �������� ������
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // ������� ��������� ������� ������
            camera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // ������� ��������� ����������
            float newWidth = Mathf.Lerp(initialResolution.x, targetResolution.x, t);
            float newHeight = Mathf.Lerp(initialResolution.y, targetResolution.y, t);

            // ������������� ����� ����������
            ppc.refResolutionX = Mathf.RoundToInt(newWidth);
            ppc.refResolutionY = Mathf.RoundToInt(newHeight);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� ���������� �����
        }

        // ������������� ��������� ��������
        camera.transform.position = targetPosition;
        ppc.refResolutionX = Mathf.RoundToInt(targetResolution.x);
        ppc.refResolutionY = Mathf.RoundToInt(targetResolution.y);
    }
}