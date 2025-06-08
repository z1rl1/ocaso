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
    public Button[] skinButtons; // ������ ������ ��� ������ ������
    public string selectedColorHex = "#FF00C5"; // ���� ���������� ����� � ������� hex
    public string defaultColorHex = "#040089"; // ���� ��������������� ����� � ������� hex
    public bool[] SkinOpen = new bool[5];
    public Image[] videoIcons; // ������ ����������� ������� ����� �� �������
    private Color selectedColor;
    private Color defaultColor;
    private bool skin0;
    private bool skin1;
    private bool skin2;
    private bool skin3;
    private bool skin4;
    private bool skin5;

    public GameObject confirmPanel; // ������ � ��������
    public Button yesButton; // ������ "��"
    public Button noButton; // ������ "���"

    private int skinIndexToUnlock; // ������ �����, ������� ����� ��������������

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            // ��������� ������ � ������
            LoadSaveCloud();
        }

        // ������ � ����������� ������
        bool[] skins = { skin0, skin1, skin2, skin3, skin4, skin5 };

        // �������� �� ���� ������
        for (int i = 0; i < skins.Length; i++)
        {
            // ���� ���� �������������, �������� ������ �����, ����� ����������
            if (skins[i])
            {
                videoIcons[i].enabled = false; // ������ ������ �����
            }
            else
            {
                videoIcons[i].enabled = true; // ���������� ������ �����
            }
        }

        // ����������� ����� �� hex-����� � Color
        ColorUtility.TryParseHtmlString(selectedColorHex, out selectedColor);
        ColorUtility.TryParseHtmlString(defaultColorHex, out defaultColor);

        // ���������� ����� ������ � ������
        UpdateButtonImages(selectedSkinIndex);

        // �������� ������� ������
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    // ��������� ������ � ������
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
                videoIcons[i].enabled = false; // ������ ������ �����
            }
            else
            {
                videoIcons[i].enabled = true; // ���������� ������ �����
            }
        }
    }

    // ��������� ������ � ������
    public void MySave(int id)
    {
        // ��������� ������ ��� ����� � ����������� �� ID
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

    // ���������� ����� �� ������ �����
    public void SelectSkin(int index)
    {
        // ���� ���� ��� �������������, ����� ���������� ���
        if (IsSkinUnlocked(index))
        {
            MySave(index); // ��������� ����� �����
            UpdateButtonImages(index); // ��������� ������� ��� ������
        }
        else
        {
            // ���� ���� �� �������������, ���������� ���� � ��������
            skinIndexToUnlock = index; // ��������� ������ ����� ��� �������������
            ShowConfirmationPanel(); // ���������� ������ � ��������������
        }
    }

    // ���������, ������������� �� ����
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

    // �������� ������ � ��������������
    private void ShowConfirmationPanel()
    {
        confirmPanel.SetActive(true); // ���������� ������
    }

    // ���������� ������ "��"
    private void OnYesClicked()
    {
        // ���������� �������
        YandexGame.RewVideoShow(skinIndexToUnlock);

        // ��������� ������ ����� ��������� �������
        MySave(skinIndexToUnlock);

        // ������ ������ �������������
        confirmPanel.SetActive(false);

        // �������� ������ ������
        UpdateButtonImages(skinIndexToUnlock);
    }

    // ���������� ������ "���"
    private void OnNoClicked()
    {
        // ������ �������� ������ �������������
        confirmPanel.SetActive(false);
    }

    // ���������� �������� ���� ������ � ����������� �� ���������� �����
    private void UpdateButtonImages(int id)
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            Image buttonImage = skinButtons[i].GetComponent<Image>(); // �������� ��������� Image

            // ���� ������ �������, ������������� ���� ���������� �����
            if (i == id)
            {
                buttonImage.color = selectedColor; // ���� ���������� ����� (FF00C5)
            }
            else // ���� ������ �� �������, ������������� ���� �� ���������
            {
                buttonImage.color = defaultColor; // ���� ��������������� ����� (040089)
            }
        }
    }
}
