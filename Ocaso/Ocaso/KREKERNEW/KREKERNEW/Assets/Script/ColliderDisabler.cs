using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDisabler : MonoBehaviour
{
    public GameObject[] objectsToDisableColliders; // Список объектов, для которых нужно отключить коллайдеры

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            DisableColliders();
        }
    }

    private void DisableColliders()
    {
        foreach (GameObject obj in objectsToDisableColliders)
        {
            PolygonCollider2D collider = obj.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                collider.enabled = false; // Отключаем коллайдер
            }
        }
    }

}
