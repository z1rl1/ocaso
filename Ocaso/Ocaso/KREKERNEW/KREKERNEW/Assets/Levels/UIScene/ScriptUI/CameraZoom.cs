using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.TextCore.Text;

public class CameraZoom : MonoBehaviour
{
    public Camera camera; // Ссылка на камеру
    public PixelPerfectCamera ppc; // Ссылка на компонент PixelPerfectCamera
    public Vector3 targetPosition = new Vector3(-389f, 48f, 0f); // Целевая позиция камеры
    public Vector2 targetResolution = new Vector2(1920f, 1080f); // Целевое разрешение
    public float duration = 2f; // Время анимации изменения разрешения и позиции
    public float delayTime = 1f; // Время задержки перед началом анимации

    private Vector2 initialResolution; // Начальное разрешение
    private Vector3 initialPosition; // Начальная позиция камеры

    void Start()
    {
        initialResolution = new Vector2(ppc.refResolutionX, ppc.refResolutionY);
        initialPosition = camera.transform.position;

        StartCoroutine(CameraTransition());
    }

    IEnumerator CameraTransition()
    {
        // Задержка перед началом анимации
        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;

        // Плавная анимация камеры
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Плавное изменение позиции камеры
            camera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Плавное изменение разрешения
            float newWidth = Mathf.Lerp(initialResolution.x, targetResolution.x, t);
            float newHeight = Mathf.Lerp(initialResolution.y, targetResolution.y, t);

            // Устанавливаем новое разрешение
            ppc.refResolutionX = Mathf.RoundToInt(newWidth);
            ppc.refResolutionY = Mathf.RoundToInt(newHeight);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующего кадра
        }

        // Устанавливаем финальные значения
        camera.transform.position = targetPosition;
        ppc.refResolutionX = Mathf.RoundToInt(targetResolution.x);
        ppc.refResolutionY = Mathf.RoundToInt(targetResolution.y);
    }
}