using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider : MonoBehaviour
{
    public GameObject[] objectsToDisableColliders; // Список объектов, для которых нужно отключить коллайдеры

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            ActivateColliders();
        }
    }


    private void ActivateColliders()
    {
        foreach (GameObject obj in objectsToDisableColliders)
        {
            PolygonCollider2D collider = obj.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                collider.enabled = true; // Отключаем коллайдер
            }
        }
    }
}
