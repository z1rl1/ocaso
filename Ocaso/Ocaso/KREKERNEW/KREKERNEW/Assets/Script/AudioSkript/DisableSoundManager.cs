using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisableSoundManager : MonoBehaviour
{
    public Button soundToggleButton; // Кнопка переключения звука
    public TextMeshProUGUI soundToggleText; // Текст на кнопке
    private bool isSoundOn; // Состояние звука

    void Start()
    {
        // Загружаем состояние звука из PlayerPrefs при старте
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        UpdateSoundState();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn; // Переключаем состояние звука
        UpdateSoundState();

        // Сохраняем состояние звука в PlayerPrefs
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateSoundState()
    {
        AudioListener.volume = isSoundOn ? 1 : 0;
        soundToggleText.text = isSoundOn ? "Выключить звук" : "Включить звук";

        // Добавим отладочный вывод в консоль для проверки состояния
        Debug.Log("Sound is " + (isSoundOn ? "on" : "off"));
    }
}


