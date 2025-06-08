using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public CanvasGroup pausePanel; // Убедитесь, что здесь CanvasGroup, а не GameObject
    private bool isPaused = false;
    private bool isSound;

    void Start()
    {
        // Инициализация кнопки паузы
        pausePanel.alpha = 0f; // Установить альфа в 0, чтобы скрыть панель
        pausePanel.interactable = false; // Запретить взаимодействие
        pausePanel.blocksRaycasts = false; // Разрешить лучи проходить

    }

    void Update()
    {
        // Пример того, как мы можем переключать паузу
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        AudioListener.pause = isSound;
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // Переключаем состояние паузы

        
        if (isPaused)
        {
            ShowPausePanel();
        }
        else
        {
            HidePausePanel();
        }
    }

    public void ShowPausePanel()
    {
        isSound = true;
        pausePanel.alpha = 1f; // Установить альфа в 1, чтобы сделать панель видимой
        pausePanel.interactable = true; // Позволить взаимодействие
        pausePanel.blocksRaycasts = true; // Блокировать лучи
        Time.timeScale = 0f; // Остановить игру (если это необходимо)

        Debug.Log("ShowPausePanel");
    }

    public void HidePausePanel()
    {
        Debug.Log("HidePausePanel вызывается");

        isSound = false;
        pausePanel.alpha = 0f; // Установить альфа в 0, чтобы скрыть панель
        pausePanel.interactable = false; // Запретить взаимодействие
        pausePanel.blocksRaycasts = false; // Разрешить лучи проходить

        Debug.Log("Проверка времени игры: " + Time.timeScale);
        Time.timeScale = 1f; // Возобновить игру (если это необходимо)

        Debug.Log("После восстановления игры, время: " + Time.timeScale);
    }

}