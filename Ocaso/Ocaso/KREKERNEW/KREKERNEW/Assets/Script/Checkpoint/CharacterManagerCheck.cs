using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerCheck : MonoBehaviour
{
    private float checkInterval = 5f;
    private float timer;
    public Hero hero; // ���������� ��� ������ �� ������ Hero

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
        Debug.Log("��� ����������! ��������� ��������.");

        if (hero != null)
        {
            hero.numberOfClonesAlive++;
            StartCoroutine(hero.FadeAndDelay());
        }
    }
}
