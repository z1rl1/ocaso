using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class UIManager : MonoBehaviour
{
    // Ссылки на панели
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject skinsPanel;

    // Ссылки на кнопки
    public Button playButton;
    public Button skinsButton;
    public Button optionsButton;
    public Button backButton; // Добавлено

    // Параметры анимации
    public float quickScaleTime = 0.1f;  // Время анимации быстрого увеличения
    public float slowScaleTime = 0.4f;   // Время анимации медленного уменьшения
    public Vector3 buttonScale = new Vector3(1.2f, 1.2f, 1f);  // Масштаб увеличения кнопки
    public float minScaleFactor = 0.5f;  // Масштаб уменьшения кнопки относительно увеличенного размера

    public float buttonAnimationTime = 0.5f;  // Время анимации кнопок
    public float buttonAlphaStart = 0f;       // Начальная прозрачность кнопок
    public float buttonAlphaEnd = 1f;         // Конечная прозрачность кнопок
    public Image fadeImage;

    public Image[] levelProgressImages;  // Ссылка на 11 изображений прогресса
    public Image[] levelCircles;          // Ссылка на кружки уровня
    public Color completedColor = new Color(1, 1, 1, 1);  // Цвет для пройденных уровней (белый)
    public Color currentColor = new Color(0, 0, 1, 1);    // Цвет для текущего уровня (синий)
    public Color uncompletedColor = new Color(0.02f, 0.02f, 0.02f, 1); // Цвет для непройденных уровней (серый)

    public Button levelButton1;
    public Button levelButton2;
    public Button levelButton3;
    public Button levelButton4;
    public Button levelButton5;
    public Button levelButton6;
    public Button levelButton7;
    public Button levelButton8;


    private bool canInteractWithUI = false;  // Флаг, который запрещает взаимодействие с UI до окончания анимации
    private bool isLevelLoading = false;

    void Start()
    {
        // Отключаем взаимодействие с UI в начале
        canInteractWithUI = false;

        // Запускаем корутину, которая включит возможность взаимодействия спустя заданное время
        StartCoroutine(EnableUIInteractionAfterDelay(5f));
    }

    // Эта корутина будет ждать, а затем включать возможность взаимодействия с UI
    private IEnumerator EnableUIInteractionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Разрешаем взаимодействие с UI
        canInteractWithUI = true;
    }


    public void ShowPlay()
    {
        if (canInteractWithUI)  // Проверяем, можем ли мы взаимодействовать с UI
        {
            StartCoroutine(AnimateUI(playButton, playPanel));
            StartCoroutine(AnimateButtonsInPanel(playPanel));
        }
    }

    public void ShowSkins()
    {
        if (canInteractWithUI) 
        {
            StartCoroutine(AnimateUI(skinsButton, skinsPanel));
            StartCoroutine(AnimateButtonsInPanel(skinsPanel));
        }
    }


    public void ShowMainMenu()
    {
        if (canInteractWithUI) 
        {
            StartCoroutine(AnimateUI(backButton, mainMenuPanel));
        }
    }


    private IEnumerator AnimateUI(Button clickedButton, GameObject panelToShow)
    {
        if (clickedButton != backButton)
        {

        }
        else 
        { 
            StartCoroutine(AnimateButtonsInPanelSecond(playPanel));
            StartCoroutine(AnimateButtonsInPanelSecond(skinsPanel));
        }

        // Показ нужной панели
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(false);
        skinsPanel.SetActive(false);
        panelToShow.SetActive(true);

        yield return null;
    }

    


    private IEnumerator AnimateButtonsInPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.4f);
        // Получаем все кнопки внутри панели
        Button[] buttons = panel.GetComponentsInChildren<Button>();

        // Создаем корутины для анимации каждой кнопки
        List<IEnumerator> animations = new List<IEnumerator>();
        float delayBetweenButtons = 0.1f;  // Задержка между кнопками
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            // Проверяем, не является ли кнопка кнопкой "Назад" и если это панель playPanel


            RectTransform rt = button.GetComponent<RectTransform>();
            Image image = button.image;

            // Настроим начальные значения
            rt.localScale = Vector3.zero; // Начальный масштаб
            Color color = image.color;
            color.a = buttonAlphaStart; // Начальная прозрачность
            image.color = color;

            // Добавляем анимацию кнопки в список корутин с задержкой
            yield return new WaitForSeconds(delayBetweenButtons);
            StartCoroutine(AnimateSingleButton(rt, image));
        }
    }

    private IEnumerator AnimateSingleButton(RectTransform rt, Image image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < buttonAnimationTime)
        {
            float progress = elapsedTime / buttonAnimationTime;
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            Color color = image.color;
            color.a = Mathf.Lerp(buttonAlphaStart, buttonAlphaEnd, progress);
            image.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедитесь, что финальные значения установлены
        rt.localScale = Vector3.one;
        Color finalColor = image.color;
        finalColor.a = buttonAlphaEnd;
        image.color = finalColor;
    }



    private IEnumerator AnimateButtonsInPanelSecond(GameObject panel)
    {
        yield return new WaitForSeconds(0.4f);
        // Получаем все кнопки внутри панели
        Button[] buttons = panel.GetComponentsInChildren<Button>();

        // Создаем корутины для анимации каждой кнопки
        List<IEnumerator> animations = new List<IEnumerator>();
        float delayBetweenButtons = 0.1f;  // Задержка между кнопками
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            RectTransform rt = button.GetComponent<RectTransform>();
            Image image = button.image;

            // Настроим начальные значения
            rt.localScale = Vector3.one; // Начальный масштаб
            Color color = image.color;
            color.a = buttonAlphaEnd; // Начальная прозрачность
            image.color = color;

            // Убедитесь, что финальные значения установлены
            rt.localScale = Vector3.zero;
            Color finalColor = image.color;
            finalColor.a = buttonAlphaStart;
            image.color = finalColor;
        }
    }



    // Функции для загрузки уровней
    public void loadLvl1()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed1;
        }
    }

    public void loadLvl2()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed2;
        }
    }

    public void loadLvl3()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed3;
        }
    }

    public void loadLvl4()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed4;
        }
    }

    public void loadLvl5()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed5;
        }
    }

    public void loadLvl6()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed6;
        }
    }

    public void loadLvl7()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed7;
        }
    }

    public void loadLvl8()
    {
        if (!isLevelLoading)  // Проверяем, не загружается ли уровень
        {
            // Показать рекламу
            YandexGame.FullscreenShow();

            // Подписываемся на событие закрытия рекламы
            YandexGame.CloseFullAdEvent += OnAdClosed8;
        }
    }

    private void OnAdClosed1()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed1;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl1", 1));
    }

    private void OnAdClosed2()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed2;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl2", 2));
    }

    private void OnAdClosed3()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed3;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl3", 3));
    }

    private void OnAdClosed4()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed4;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl4", 4));
    }

    private void OnAdClosed5()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed5;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl5", 5));
    }

    // Событие, которое срабатывает после того, как реклама была закрыта
    private void OnAdClosed6()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed6;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl6", 6));
    }

    private void OnAdClosed7()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed7;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl7", 7));
    }

    private void OnAdClosed8()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed8;

        // Запускаем анимацию и загрузку уровня
        StartCoroutine(FadeAndLoadScene("lvl8", 8));
    }

    


    // Функция для изменения доступности кнопок уровней
    private void SetLevelButtonsInteractable(bool interactable)
    {
        levelButton1.interactable = interactable;
        levelButton2.interactable = interactable;
        levelButton3.interactable = interactable;
        levelButton4.interactable = interactable;
        levelButton5.interactable = interactable;
        levelButton6.interactable = interactable;
        levelButton7.interactable = interactable;
        levelButton8.interactable = interactable;

    }

    private IEnumerator FadeAndLoadScene(string sceneName, int currentLevel)
    {
        // Запрещаем взаимодействие с кнопками уровней
        isLevelLoading = true;
        SetLevelButtonsInteractable(false);  // Отключаем кнопки

        // Запускаем анимацию затемнения
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / 1f);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;  
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;

        // Показываем прогресс
        yield return StartCoroutine(ShowLevelProgress(currentLevel));

        // Загружаем сцену
        SceneManager.LoadScene(sceneName);

        // Разрешаем взаимодействие с кнопками после загрузки сцены
        yield return new WaitForSeconds(0.5f);  // Задержка для плавного завершения загрузки
        SetLevelButtonsInteractable(true);
        isLevelLoading = false;
    }




    private IEnumerator AnimateColorTransition(Image image, Color targetColor, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;  // Убедитесь, что цвет установлен в конечное значение
    }

    private IEnumerator UpdateLevelProgress(int currentLevel)
    {
        float transitionDuration = 0.5f; // Время анимации изменения цвета

        // Обновление цветов кружков
        for (int i = 0; i < levelCircles.Length; i++)
        {
            Color targetColor;

            if (i < currentLevel - 1)
            {
                // Уровни, которые пройдены
                targetColor = completedColor;
            }
            else if (i == currentLevel - 1)
            {
                // Текущий уровень
                targetColor = currentColor;
            }
            else
            {
                // Не пройденные уровни
                targetColor = uncompletedColor;
            }

            // Плавное изменение цвета
            yield return StartCoroutine(AnimateColorTransition(levelCircles[i], targetColor, transitionDuration));
        }
    }


    private IEnumerator ShowLevelProgress(int currentLevel)
    {
        // Показ изображений прогресса
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 0;
            img.color = color;
        }

        // Появление изображений прогресса
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Время для появления изображений

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            foreach (Image img in levelProgressImages)
            {
                Color color = img.color;
                color.a = alpha;
                img.color = color;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедитесь, что альфа-канал установлен в 1
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 1;
            img.color = color;
        }

        // Обновите прогресс уровней с плавным изменением цвета
        yield return StartCoroutine(UpdateLevelProgress(currentLevel));
    }





}