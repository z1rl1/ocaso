using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    public int numberOfClonesAlive = 0;

    // Метод для увеличения количества клонов
    public void AddClone()
    {
        numberOfClonesAlive++;
        Debug.Log("Клонов: " + numberOfClonesAlive);
    }

    // Метод для уменьшения количества клонов
    public void RemoveClone()
    {
        numberOfClonesAlive--;
        Debug.Log("Клонов: " + numberOfClonesAlive);

        // Если клонов больше нет, вызываем необходимую логику
        if (numberOfClonesAlive <= 0)
        {
            // Ваш код для окончания игры или перехода на следующий уровень
            Debug.Log("Все клоны погибли!");
        }
    }
}
