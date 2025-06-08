using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDynObjLvl3DieZone : MonoBehaviour
{
    public string targetTag = "DieZone"; // Тег объектов, которые будут опускаться
    public float dropDistance = 10f; // Расстояние, на которое нужно опустить объекты
    public float dropDuration = 5f; // Время, за которое объекты должны опуститься
    public float raiseDuration = 5f; // Время, за которое объекты должны подняться
    public float waitTime = 1f; // Время ожидания перед подъемом

    private bool isActivated = false; // Флаг активации триггера

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && !isActivated) // Проверяем, что это персонаж и триггер еще не активирован
        {
            isActivated = true; // Устанавливаем флаг, чтобы предотвратить повторную активацию
            StartCoroutine(HandleObjects()); // Запускаем корутину для обработки объектов
        }
    }

    private IEnumerator HandleObjects()
    {
        GameObject[] objectsToHandle = GameObject.FindGameObjectsWithTag(targetTag); // Находим все объекты с заданным тегом

        // Запускаем корутину для каждого объекта
        foreach (GameObject obj in objectsToHandle)
        {
            StartCoroutine(DropAndRaiseObject(obj.transform));
        }

        yield return null; // Завершаем корутину
    }

    private IEnumerator DropAndRaiseObject(Transform objTransform)
    {
        while (true) // Бесконечный цикл
        {
            Vector3 startPos = objTransform.position;
            Vector3 dropPos = new Vector3(startPos.x, startPos.y - dropDistance, startPos.z);
            float elapsedTime = 0f;

            // Плавно опускаем объект
            while (elapsedTime < dropDuration)
            {
                objTransform.position = Vector3.Lerp(startPos, dropPos, elapsedTime / dropDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // Ждем до следующего кадра
            }

            objTransform.position = dropPos;

            // Ждем перед подъемом
            yield return new WaitForSeconds(waitTime);

            // Плавно поднимаем объект обратно
            elapsedTime = 0f; // Сброс времени
            while (elapsedTime < raiseDuration)
            {
                objTransform.position = Vector3.Lerp(dropPos, startPos, elapsedTime / raiseDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // Ждем до следующего кадра
            }

            objTransform.position = startPos;

            // Ждем перед началом следующего цикла
            yield return new WaitForSeconds(waitTime);
        }
    }
}
        
