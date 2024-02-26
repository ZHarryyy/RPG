using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
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

    private void Awake()
    {
        skillTreeUI.SetActive(true);
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
                SetInGameUIVisible(alwaysShowInGameUI);
            }
            else if (isAnyUIScreenOpen)
            {
                CloseAllUI();
                SetInGameUIVisible(alwaysShowInGameUI);
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
        isAnyUIScreenOpen = ui.activeSelf;
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
}
