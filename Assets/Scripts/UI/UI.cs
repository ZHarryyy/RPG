using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] UIScreens;
    private int currentScreenIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchToPreviousScreen();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchToNextScreen();
        }
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

    // public void SwitchTo(GameObject _menu)
    // {
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         transform.GetChild(i).gameObject.SetActive(false);
    //     }

    //     if (_menu != null) _menu.SetActive(true);
    // }
}
