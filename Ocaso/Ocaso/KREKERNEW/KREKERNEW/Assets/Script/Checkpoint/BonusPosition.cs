using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPosition : MonoBehaviour
{
    public Vector3 initialPosition;
    public bool isPicked = false; // Добавьте это поле
    private void Start()
    {
        initialPosition = transform.position; // Сохраняем первоначальную позицию при создании бонуса
    }
}