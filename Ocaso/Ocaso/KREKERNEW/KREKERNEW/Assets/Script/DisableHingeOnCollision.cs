using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHingeOnCollision : MonoBehaviour
{
    // Ссылка на Hinge Joint 2D компонент для каждого звена цепи
    public HingeJoint2D[] hingeJoints;

    // Размер персонажа, при котором нужно выключать Hinge Joint
    public Vector3 characterSize = new Vector3(0.2582829f, 0.2582829f, 0.2582829f);

    // Функция вызывается, когда объект входит в контакт с другим коллайдером
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем тег коллайдера
        if (collision.collider.CompareTag("Character"))
        {
            // Проверяем размер персонажа
            if (Mathf.Approximately(collision.collider.transform.localScale.x, characterSize.x) &&
                Mathf.Approximately(collision.collider.transform.localScale.y, characterSize.y) &&
                Mathf.Approximately(collision.collider.transform.localScale.z, characterSize.z))
            {
                // Выключаем Hinge Joint для каждого звена цепи
                foreach (var hingeJoint in hingeJoints)
                {
                    hingeJoint.enabled = false;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            // Проверяем размер персонажа
            if (Mathf.Approximately(collision.transform.localScale.x, characterSize.x) &&
                Mathf.Approximately(collision.transform.localScale.y, characterSize.y) &&
                Mathf.Approximately(collision.transform.localScale.z, characterSize.z))
            {
                // Выключаем Hinge Joint для каждого звена цепи
                foreach (var hingeJoint in hingeJoints)
                {
                    hingeJoint.enabled = false;
                }
            }
        }
    }


}