using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class SkinManager : MonoBehaviour
{
    public RuntimeAnimatorController[] skinControllers;
    private int selectedSkinIndex;
    public Button[] skinButtons; // Массив кнопок для выбора скинов
    public string selectedColorHex = "#FF00C5"; // Цвет выбранного скина в формате hex
    public string defaultColorHex = "#040089"; // Цвет незакрепленного скина в формате hex
    public bool[] SkinOpen = new bool[5];
    public Image[] videoIcons; // Массив изображений значков видео на кнопках
    private Color selectedColor;
    private Color defaultColor;
    private bool skin0;
    private bool skin1;
    private bool skin2;
    private bool skin3;
    private bool skin4;
    private bool skin5;

    public GameObject confirmPanel; // Панель с вопросом
    public Button yesButton; // Кнопка "Да"
    public Button noButton; // Кнопка "Нет"

    private int skinIndexToUnlock; // Индекс скина, который нужно разблокировать

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            // Загружаем данные с облака
            LoadSaveCloud();
        }

        // Массив с состояниями скинов
        bool[] skins = { skin0, skin1, skin2, skin3, skin4, skin5 };

        // Проходим по всем скинам
        for (int i = 0; i < skins.Length; i++)
        {
            // Если скин разблокирован, скрываем значок видео, иначе показываем
            if (skins[i])
            {
                videoIcons[i].enabled = false; // Прячем значок видео
            }
            else
            {
                videoIcons[i].enabled = true; // Показываем значок видео
            }
        }

        // Преобразуем цвета из hex-строк в Color
        ColorUtility.TryParseHtmlString(selectedColorHex, out selectedColor);
        ColorUtility.TryParseHtmlString(defaultColorHex, out defaultColor);

        // Обновление цвета кнопок в начале
        UpdateButtonImages(selectedSkinIndex);

        // Привязка событий кнопок
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    // Загружаем данные с облака
    public void LoadSaveCloud()
    {
        skin0 = YandexGame.savesData.skin0;
        skin1 = YandexGame.savesData.skin1;
        skin2 = YandexGame.savesData.skin2;
        skin3 = YandexGame.savesData.skin3;
        skin4 = YandexGame.savesData.skin4;
        skin5 = YandexGame.savesData.skin5;

        selectedSkinIndex = YandexGame.savesData.pickSkin;

    }

    private void UpdateVideoIcons()
    {
        bool[] skins = {skin0, skin1, skin2, skin3, skin4, skin5 };

        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i])
            {
                videoIcons[i].enabled = false; // Прячем значок видео
            }
            else
            {
                videoIcons[i].enabled = true; // Показываем значок видео
            }
        }
    }

    // Сохраняем данные в облаке
    public void MySave(int id)
    {
        // Сохраняем данные для скина в зависимости от ID
        if (id == 0) { skin0 = true; YandexGame.savesData.skin0 = skin0; }
        if (id == 1) { skin1 = true; YandexGame.savesData.skin1 = skin1; }
        if (id == 2) { skin2 = true; YandexGame.savesData.skin2 = skin2; }
        if (id == 3) { skin3 = true; YandexGame.savesData.skin3 = skin3; }
        if (id == 4) { skin4 = true; YandexGame.savesData.skin4 = skin4; }
        if (id == 5) { skin5 = true; YandexGame.savesData.skin5 = skin5; }

        UpdateVideoIcons();

        YandexGame.savesData.pickSkin = id;
        YandexGame.SaveProgress();
    }

    // Обработчик клика по кнопке скина
    public void SelectSkin(int index)
    {
        // Если скин уже разблокирован, сразу показываем его
        if (IsSkinUnlocked(index))
        {
            MySave(index); // Сохраняем выбор скина
            UpdateButtonImages(index); // Обновляем внешний вид кнопок
        }
        else
        {
            // Если скин не разблокирован, показываем окно с вопросом
            skinIndexToUnlock = index; // Сохраняем индекс скина для разблокировки
            ShowConfirmationPanel(); // Показываем панель с подтверждением
        }
    }

    // Проверяем, разблокирован ли скин
    private bool IsSkinUnlocked(int index)
    {
        if (index == 0) return skin0;
        if (index == 1) return skin1;
        if (index == 2) return skin2;
        if (index == 3) return skin3;
        if (index == 4) return skin4;
        if (index == 5) return skin5;
        return false;
    }

    // Показать панель с подтверждением
    private void ShowConfirmationPanel()
    {
        confirmPanel.SetActive(true); // Активируем панель
    }

    // Обработчик кнопки "Да"
    private void OnYesClicked()
    {
        // Показываем рекламу
        YandexGame.RewVideoShow(skinIndexToUnlock);

        // Сохраняем данные после просмотра рекламы
        MySave(skinIndexToUnlock);

        // Скрыть панель подтверждения
        confirmPanel.SetActive(false);

        // Обновить кнопки скинов
        UpdateButtonImages(skinIndexToUnlock);
    }

    // Обработчик кнопки "Нет"
    private void OnNoClicked()
    {
        // Просто скрываем панель подтверждения
        confirmPanel.SetActive(false);
    }

    // Обновление внешнего вида кнопок в зависимости от выбранного скина
    private void UpdateButtonImages(int id)
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            Image buttonImage = skinButtons[i].GetComponent<Image>(); // Получаем компонент Image

            // Если кнопка выбрана, устанавливаем цвет выбранного скина
            if (i == id)
            {
                buttonImage.color = selectedColor; // Цвет выбранного скина (FF00C5)
            }
            else // Если кнопка не выбрана, устанавливаем цвет по умолчанию
            {
                buttonImage.color = defaultColor; // Цвет незакрепленного скина (040089)
            }
        }
    }
}
