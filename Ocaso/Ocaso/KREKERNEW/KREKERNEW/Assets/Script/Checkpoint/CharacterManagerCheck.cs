using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerCheck : MonoBehaviour
{
    private float checkInterval = 5f;
    private float timer;
    public Hero hero; // Переменная для ссылки на объект Hero

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;

            if (GameObject.FindGameObjectsWithTag("Character").Length == 0)
            {
                ExecuteAction();
            }
        }
    }

    private void ExecuteAction()
    {
        Debug.Log("Нет персонажей! Выполняем действие.");

        if (hero != null)
        {
            hero.numberOfClonesAlive++;
            StartCoroutine(hero.FadeAndDelay());
        }
    }
}
