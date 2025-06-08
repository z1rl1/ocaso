using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivatorStickyBall2 : MonoBehaviour
{

    public List<NotInfShoot> guns;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            foreach (var gun in guns)
            {
                if (gun != null)
                {
                    gun.EnableShooting();
                }
            }
            gameObject.SetActive(false); // Отключаем триггер
        }
    }

}
