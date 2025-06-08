using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject spriteObject;
    public GameObject objectToDisable;
    private bool istake = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && istake == false)
        {
            istake = true;
            Vector3 spawnPosition = spriteObject.transform.position;
            Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

            Collider2D collider = objectToDisable.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}
