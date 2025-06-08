using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons; // Массив кнопок уровней
    int highestLevelReached;

    void Start()
    {
        // Получаем номер самого высокого уровня, который был пройден
        //int highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);

        if (YandexGame.SDKEnabled)
        {
            // Загружаем данные с облака
            LoadSaveCloud();
        }


        // Настраиваем кнопки уровней
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Color buttonColor;
            Color textColor;

            if (i + 1 <= highestLevelReached)
            {
                levelButtons[i].interactable = true; // Делаем уровень доступным

                ColorUtility.TryParseHtmlString("#040089", out buttonColor); // Цвет кнопки
                ColorUtility.TryParseHtmlString("#3D8DFF", out textColor);    // Цвет текста
            }
            else
            {
                levelButtons[i].interactable = false; // Блокируем уровень
                ColorUtility.TryParseHtmlString("#270080", out buttonColor); // Цвет кнопки
                ColorUtility.TryParseHtmlString("#002B68", out textColor);    // Цвет текста
            }

            levelButtons[i].image.color = buttonColor; // Цвет фона кнопки

            // Проверяем, есть ли текстовый компонент TextMeshPro или Text
            TMP_Text buttonText = levelButtons[i].GetComponentInChildren<TMP_Text>(); // Получаем текст внутри кнопки
            if (buttonText == null)
            {
                // Если TMP_Text не найден, попробуем найти обычный Text
                buttonText = levelButtons[i].GetComponentInChildren<TMP_Text>();
            }

            if (buttonText != null)
            {
                buttonText.color = textColor; // Устанавливаем цвет текста
            }

        }
    }

    public void LoadSaveCloud()
    {
        highestLevelReached = YandexGame.savesData.HighestLevelReached;

    }



    // Метод для загрузки уровня (назначается на кнопки)
    public void LoadLevel(int levelIndex)
    {
        string levelName = $"lvl{levelIndex}"; // Имя сцены
        SceneManager.LoadScene(levelName); // Загружаем уровень
    }
}