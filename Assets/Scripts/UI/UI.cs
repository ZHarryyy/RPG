using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, ISaveManager
{
    [SerializeField] private GameObject[] UIScreens;
    private int currentScreenIndex;

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    public UI_ItemToolTip itemToolTip;
    public UI_StatTooltip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    public Toggle toggleInGameUI;

    private bool isAnyUIScreenOpen = false;
    public bool alwaysShowInGameUI;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        skillTreeUI.SetActive(true);
        fadeScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        CloseAllUI();
        alwaysShowInGameUI = toggleInGameUI.isOn;
        SetInGameUIVisible(alwaysShowInGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !characterUI.activeSelf && !optionsUI.activeSelf) ToggleUI(characterUI);
        else if (Input.GetKeyDown(KeyCode.I) && !skillTreeUI.activeSelf && !optionsUI.activeSelf) ToggleUI(skillTreeUI);
        else if (Input.GetKeyDown(KeyCode.C) && !craftUI.activeSelf && !optionsUI.activeSelf) ToggleUI(craftUI);
        else if (Input.GetKeyDown(KeyCode.Q) && isAnyUIScreenOpen) SwitchToPreviousScreen();
        else if (Input.GetKeyDown(KeyCode.E) && isAnyUIScreenOpen) SwitchToNextScreen();
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isAnyUIScreenOpen)
            {
                ToggleUI(optionsUI);
            }
            else if (currentScreenIndex == 0 && optionsUI.activeSelf)
            {
                CloseAllUI();
                inGameUI.SetActive(true);
                AudioManager.instance.PlaySFX(7, null);
                SetInGameUIVisible(alwaysShowInGameUI);
                if (GameManager.instance != null) GameManager.instance.PauseGame(false);
            }
            else if (isAnyUIScreenOpen)
            {
                CloseAllUI();
                AudioManager.instance.PlaySFX(7, null);
                SetInGameUIVisible(alwaysShowInGameUI);
                if (GameManager.instance != null) GameManager.instance.PauseGame(false);
            }
        }

        alwaysShowInGameUI = toggleInGameUI.isOn;
    }

    private void SetInGameUIVisible(bool _isShow)
    {
        if (alwaysShowInGameUI == true) inGameUI.SetActive(true);
        else inGameUI.SetActive(false);
    }

    private void CloseAllUI()
    {
        characterUI.SetActive(false);
        skillTreeUI.SetActive(false);
        craftUI.SetActive(false);
        optionsUI.SetActive(false);
        isAnyUIScreenOpen = false;
    }

    private void ToggleUI(GameObject ui)
    {
        bool wasAnyUIScreenOpen = isAnyUIScreenOpen;

        CloseAllUI();
        SetInGameUIVisible(alwaysShowInGameUI);

        if (!wasAnyUIScreenOpen || ui != UIScreens[currentScreenIndex])
        {
            for (int i = 0; i < UIScreens.Length; i++)
            {
                if (UIScreens[i] == ui)
                {
                    currentScreenIndex = i;
                    break;
                }
            }
        }

        ui.SetActive(!ui.activeSelf);
        AudioManager.instance.PlaySFX(6, null);
        isAnyUIScreenOpen = ui.activeSelf;

        if (GameManager.instance != null) GameManager.instance.PauseGame(true);
    }

    private void SwitchToPreviousScreen()
    {
        currentScreenIndex--;
        if (currentScreenIndex < 0)
        {
            currentScreenIndex = UIScreens.Length - 1;
        }

        UpdateUI();
    }

    private void SwitchToNextScreen()
    {
        currentScreenIndex++;
        if (currentScreenIndex >= UIScreens.Length)
        {
            currentScreenIndex = 0;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < UIScreens.Length; i++)
        {
            if (i == currentScreenIndex)
            {
                UIScreens[i].SetActive(true);
            }
            else
            {
                UIScreens[i].SetActive(false);
            }
        }

        AudioManager.instance.PlaySFX(6, null);
    }

    public void SwitchOnEndScreen()
    {
        CloseAllUI();
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);

        endText.SetActive(true);

        yield return new WaitForSeconds(1);

        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key) item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
