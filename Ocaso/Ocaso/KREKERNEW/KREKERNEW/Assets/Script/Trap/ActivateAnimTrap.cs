using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimTrap : MonoBehaviour
{
    public List<GameObject> objectsToActivate; // Список объектов с аниматорами
    public float deactivateTime = 2f; // Время, на которое триггер будет отключен

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character")) // Проверяем, что объект с тегом "Character"
        {
            ActivateAnimators();
            StartCoroutine(DeactivateTriggerAndAnimators());
        }
    }

    private void ActivateAnimators()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true; // Включаем аниматор
            }
        }
    }

    private IEnumerator DeactivateTriggerAndAnimators()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false; // Отключаем триггер

            // Ждем заданное время
            yield return new WaitForSeconds(deactivateTime);

            // Отключаем аниматоры
            foreach (GameObject obj in objectsToActivate)
            {
                Animator animator = obj.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = false; // Отключаем аниматор
                }
            }

            // Включаем триггер снова
            triggerCollider.enabled = true;
        }
    }
}