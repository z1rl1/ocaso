using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerOpenTrap : MonoBehaviour
{
    private static HashSet<ButtonControllerOpenTrap> activatedButtons = new HashSet<ButtonControllerOpenTrap>();
    private Color originalColor;
    public Color activatedColor = Color.magenta; // Фиолетовый цвет
    public GameObject objectToRaise; // Объект, который будет подниматься
    public float raiseAmount = 1f; // Сколько поднимать
    public float raiseDuration = 1f; // Время подъема

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = activatedColor;
            }

            activatedButtons.Add(this);

            // Проверяем, если нажаты обе кнопки
            if (activatedButtons.Count == 2 && objectToRaise != null)
            {
                StartCoroutine(RaiseObject(objectToRaise));
            }
        }
    }

    private IEnumerator RaiseObject(GameObject obj)
    {
        Vector3 originalPosition = obj.transform.position;
        Vector3 targetPosition = originalPosition + new Vector3(0, raiseAmount, 0);
        float elapsedTime = 0f;

        while (elapsedTime < raiseDuration)
        {
            obj.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / raiseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = targetPosition;

        activatedButtons.Clear();
    }
}