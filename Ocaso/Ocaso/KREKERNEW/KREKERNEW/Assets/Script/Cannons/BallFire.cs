using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFire : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DieZone"))
        {
            Destroy(gameObject);
        }
    }
}
