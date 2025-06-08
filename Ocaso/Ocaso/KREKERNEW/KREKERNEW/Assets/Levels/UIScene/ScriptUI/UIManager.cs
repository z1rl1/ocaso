using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class UIManager : MonoBehaviour
{
    // ������ �� ������
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject skinsPanel;

    // ������ �� ������
    public Button playButton;
    public Button skinsButton;
    public Button optionsButton;
    public Button backButton; // ���������

    // ��������� ��������
    public float quickScaleTime = 0.1f;  // ����� �������� �������� ����������
    public float slowScaleTime = 0.4f;   // ����� �������� ���������� ����������
    public Vector3 buttonScale = new Vector3(1.2f, 1.2f, 1f);  // ������� ���������� ������
    public float minScaleFactor = 0.5f;  // ������� ���������� ������ ������������ ������������ �������

    public float buttonAnimationTime = 0.5f;  // ����� �������� ������
    public float buttonAlphaStart = 0f;       // ��������� ������������ ������
    public float buttonAlphaEnd = 1f;         // �������� ������������ ������
    public Image fadeImage;

    public Image[] levelProgressImages;  // ������ �� 11 ����������� ���������
    public Image[] levelCircles;          // ������ �� ������ ������
    public Color completedColor = new Color(1, 1, 1, 1);  // ���� ��� ���������� ������� (�����)
    public Color currentColor = new Color(0, 0, 1, 1);    // ���� ��� �������� ������ (�����)
    public Color uncompletedColor = new Color(0.02f, 0.02f, 0.02f, 1); // ���� ��� ������������ ������� (�����)

    public Button levelButton1;
    public Button levelButton2;
    public Button levelButton3;
    public Button levelButton4;
    public Button levelButton5;
    public Button levelButton6;
    public Button levelButton7;
    public Button levelButton8;


    private bool canInteractWithUI = false;  // ����, ������� ��������� �������������� � UI �� ��������� ��������
    private bool isLevelLoading = false;

    void Start()
    {
        // ��������� �������������� � UI � ������
        canInteractWithUI = false;

        // ��������� ��������, ������� ������� ����������� �������������� ������ �������� �����
        StartCoroutine(EnableUIInteractionAfterDelay(5f));
    }

    // ��� �������� ����� �����, � ����� �������� ����������� �������������� � UI
    private IEnumerator EnableUIInteractionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ��������� �������������� � UI
        canInteractWithUI = true;
    }


    public void ShowPlay()
    {
        if (canInteractWithUI)  // ���������, ����� �� �� ����������������� � UI
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

        // ����� ������ ������
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(false);
        skinsPanel.SetActive(false);
        panelToShow.SetActive(true);

        yield return null;
    }

    


    private IEnumerator AnimateButtonsInPanel(GameObject panel)
    {
        yield return new WaitForSeconds(0.4f);
        // �������� ��� ������ ������ ������
        Button[] buttons = panel.GetComponentsInChildren<Button>();

        // ������� �������� ��� �������� ������ ������
        List<IEnumerator> animations = new List<IEnumerator>();
        float delayBetweenButtons = 0.1f;  // �������� ����� ��������
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            // ���������, �� �������� �� ������ ������� "�����" � ���� ��� ������ playPanel


            RectTransform rt = button.GetComponent<RectTransform>();
            Image image = button.image;

            // �������� ��������� ��������
            rt.localScale = Vector3.zero; // ��������� �������
            Color color = image.color;
            color.a = buttonAlphaStart; // ��������� ������������
            image.color = color;

            // ��������� �������� ������ � ������ ������� � ���������
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

        // ���������, ��� ��������� �������� �����������
        rt.localScale = Vector3.one;
        Color finalColor = image.color;
        finalColor.a = buttonAlphaEnd;
        image.color = finalColor;
    }



    private IEnumerator AnimateButtonsInPanelSecond(GameObject panel)
    {
        yield return new WaitForSeconds(0.4f);
        // �������� ��� ������ ������ ������
        Button[] buttons = panel.GetComponentsInChildren<Button>();

        // ������� �������� ��� �������� ������ ������
        List<IEnumerator> animations = new List<IEnumerator>();
        float delayBetweenButtons = 0.1f;  // �������� ����� ��������
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            RectTransform rt = button.GetComponent<RectTransform>();
            Image image = button.image;

            // �������� ��������� ��������
            rt.localScale = Vector3.one; // ��������� �������
            Color color = image.color;
            color.a = buttonAlphaEnd; // ��������� ������������
            image.color = color;

            // ���������, ��� ��������� �������� �����������
            rt.localScale = Vector3.zero;
            Color finalColor = image.color;
            finalColor.a = buttonAlphaStart;
            image.color = finalColor;
        }
    }



    // ������� ��� �������� �������
    public void loadLvl1()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed1;
        }
    }

    public void loadLvl2()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed2;
        }
    }

    public void loadLvl3()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed3;
        }
    }

    public void loadLvl4()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed4;
        }
    }

    public void loadLvl5()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed5;
        }
    }

    public void loadLvl6()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed6;
        }
    }

    public void loadLvl7()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed7;
        }
    }

    public void loadLvl8()
    {
        if (!isLevelLoading)  // ���������, �� ����������� �� �������
        {
            // �������� �������
            YandexGame.FullscreenShow();

            // ������������� �� ������� �������� �������
            YandexGame.CloseFullAdEvent += OnAdClosed8;
        }
    }

    private void OnAdClosed1()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed1;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl1", 1));
    }

    private void OnAdClosed2()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed2;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl2", 2));
    }

    private void OnAdClosed3()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed3;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl3", 3));
    }

    private void OnAdClosed4()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed4;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl4", 4));
    }

    private void OnAdClosed5()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed5;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl5", 5));
    }

    // �������, ������� ����������� ����� ����, ��� ������� ���� �������
    private void OnAdClosed6()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed6;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl6", 6));
    }

    private void OnAdClosed7()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed7;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl7", 7));
    }

    private void OnAdClosed8()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed8;

        // ��������� �������� � �������� ������
        StartCoroutine(FadeAndLoadScene("lvl8", 8));
    }

    


    // ������� ��� ��������� ����������� ������ �������
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
        // ��������� �������������� � �������� �������
        isLevelLoading = true;
        SetLevelButtonsInteractable(false);  // ��������� ������

        // ��������� �������� ����������
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

        // ���������� ��������
        yield return StartCoroutine(ShowLevelProgress(currentLevel));

        // ��������� �����
        SceneManager.LoadScene(sceneName);

        // ��������� �������������� � �������� ����� �������� �����
        yield return new WaitForSeconds(0.5f);  // �������� ��� �������� ���������� ��������
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

        image.color = targetColor;  // ���������, ��� ���� ���������� � �������� ��������
    }

    private IEnumerator UpdateLevelProgress(int currentLevel)
    {
        float transitionDuration = 0.5f; // ����� �������� ��������� �����

        // ���������� ������ �������
        for (int i = 0; i < levelCircles.Length; i++)
        {
            Color targetColor;

            if (i < currentLevel - 1)
            {
                // ������, ������� ��������
                targetColor = completedColor;
            }
            else if (i == currentLevel - 1)
            {
                // ������� �������
                targetColor = currentColor;
            }
            else
            {
                // �� ���������� ������
                targetColor = uncompletedColor;
            }

            // ������� ��������� �����
            yield return StartCoroutine(AnimateColorTransition(levelCircles[i], targetColor, transitionDuration));
        }
    }


    private IEnumerator ShowLevelProgress(int currentLevel)
    {
        // ����� ����������� ���������
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 0;
            img.color = color;
        }

        // ��������� ����������� ���������
        float elapsedTime = 0f;
        float fadeDuration = 1f; // ����� ��� ��������� �����������

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

        // ���������, ��� �����-����� ���������� � 1
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 1;
            img.color = color;
        }

        // �������� �������� ������� � ������� ���������� �����
        yield return StartCoroutine(UpdateLevelProgress(currentLevel));
    }





}