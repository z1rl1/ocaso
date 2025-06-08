using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainMenuBack : MonoBehaviour
{

    public CanvasGroup pausePanel; // Убедитесь, что здесь CanvasGroup, а не GameObject

    void Start()
    {
        // Инициализация кнопки паузы
        pausePanel.alpha = 0f; // Установить альфа в 0, чтобы скрыть панель
        pausePanel.interactable = false; // Запретить взаимодействие
        pausePanel.blocksRaycasts = false; // Разрешить лучи проходить

    }

    public void GoToLevel0()
    {
        // Переход на сцену уровня 0
        SceneManager.LoadScene("lvl0");
        Time.timeScale = 1f;
        YandexGame.FullscreenShow();
        
    }

}
