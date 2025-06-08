using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons; // ������ ������ �������
    int highestLevelReached;

    void Start()
    {
        // �������� ����� ������ �������� ������, ������� ��� �������
        //int highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);

        if (YandexGame.SDKEnabled)
        {
            // ��������� ������ � ������
            LoadSaveCloud();
        }


        // ����������� ������ �������
        for (int i = 0; i < levelButtons.Length; i++)
        {
            Color buttonColor;
            Color textColor;

            if (i + 1 <= highestLevelReached)
            {
                levelButtons[i].interactable = true; // ������ ������� ���������

                ColorUtility.TryParseHtmlString("#040089", out buttonColor); // ���� ������
                ColorUtility.TryParseHtmlString("#3D8DFF", out textColor);    // ���� ������
            }
            else
            {
                levelButtons[i].interactable = false; // ��������� �������
                ColorUtility.TryParseHtmlString("#270080", out buttonColor); // ���� ������
                ColorUtility.TryParseHtmlString("#002B68", out textColor);    // ���� ������
            }

            levelButtons[i].image.color = buttonColor; // ���� ���� ������

            // ���������, ���� �� ��������� ��������� TextMeshPro ��� Text
            TMP_Text buttonText = levelButtons[i].GetComponentInChildren<TMP_Text>(); // �������� ����� ������ ������
            if (buttonText == null)
            {
                // ���� TMP_Text �� ������, ��������� ����� ������� Text
                buttonText = levelButtons[i].GetComponentInChildren<TMP_Text>();
            }

            if (buttonText != null)
            {
                buttonText.color = textColor; // ������������� ���� ������
            }

        }
    }

    public void LoadSaveCloud()
    {
        highestLevelReached = YandexGame.savesData.HighestLevelReached;

    }



    // ����� ��� �������� ������ (����������� �� ������)
    public void LoadLevel(int levelIndex)
    {
        string levelName = $"lvl{levelIndex}"; // ��� �����
        SceneManager.LoadScene(levelName); // ��������� �������
    }
}