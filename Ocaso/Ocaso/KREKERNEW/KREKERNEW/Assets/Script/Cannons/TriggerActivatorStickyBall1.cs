using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivatorStickyBall1 : MonoBehaviour
{
    
    public List<CannonWithInterval> guns;


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
        }
    }

}
