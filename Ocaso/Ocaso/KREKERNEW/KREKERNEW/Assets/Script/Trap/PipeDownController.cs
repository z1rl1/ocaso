using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDownController : MonoBehaviour
{
    public float fallSpeed = 2f; // Скорость падения
    public float returnSpeed = 2f; // Скорость возврата
    public float returnDelay = 2f; // Задержка перед возвратом
    public float fallAngle = 45f; // Угол падения

    private Quaternion initialRotation;
    private bool isFalling = false;
    private bool isReturning = false; // Новый флаг для отслеживания состояния возврата
    private float targetAngle;

    void Start()
    {
        initialRotation = transform.rotation; // Запоминаем начальную ротацию
    }

    void Update()
    {
        if (isFalling)
        {
            // Вращение
            targetAngle += fallSpeed * Time.deltaTime * (180f / Mathf.PI); // Преобразуем скорость в градусы
            transform.rotation = Quaternion.Euler(0, 0, initialRotation.eulerAngles.z - targetAngle);

            // Проверяем, достигла ли труба заданного угла
            if (targetAngle >= fallAngle)
            {
                isFalling = false;
                isReturning = true; // Начинаем возвращение
                StartCoroutine(ReturnToInitialPosition());
            }
        }
    }

    public void StartFalling()
    {
        if (!isFalling && !isReturning) // Падение только если труба не падает и не возвращается
        {
            isFalling = true;
            targetAngle = 0; // Начинаем с нуля
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

        transform.rotation = initialRotation; // Возвращаем точно
        isReturning = false; // Теперь труба снова готова к падению
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character")) // Проверяем, если соприкасается с персонажем
        {
            StartFalling(); // Запускаем падение
        }
    }
}