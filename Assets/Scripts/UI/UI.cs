using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] UIScreens;
    private int currentScreenIndex;

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatTooltip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    private bool isAnyUIScreenOpen = false;

    private void Awake()
    {
        skillTreeUI.SetActive(true);
    }

    private void Start()
    {
        CloseAllUI();
        inGameUI.SetActive(true);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !characterUI.activeSelf) ToggleUI(characterUI);
        else if (Input.GetKeyDown(KeyCode.I) && !skillTreeUI.activeSelf) ToggleUI(skillTreeUI);
        else if (Input.GetKeyDown(KeyCode.B) && !craftUI.activeSelf) ToggleUI(craftUI);
        else if (Input.GetKeyDown(KeyCode.Q) && isAnyUIScreenOpen) SwitchToPreviousScreen();
        else if (Input.GetKeyDown(KeyCode.E) && isAnyUIScreenOpen) SwitchToNextScreen();
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllUI();
            inGameUI.SetActive(true);
        }
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
        inGameUI.SetActive(false);

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
}
