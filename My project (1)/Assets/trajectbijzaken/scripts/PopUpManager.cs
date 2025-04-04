using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject[] popUpPanels;     // Assign in Inspector
    public GameObject[] schatkisten;     // Assign in Inspector
    public GameObject[] zandhopen;       // Assign in Inspector
    public Button[] progressButtons;     // Assign in Inspector

    void Start()
    {
        // Load and simulate saved progress on startup
        if (PlayerPrefs.HasKey("LastProgressIndex"))
        {
            int savedIndex = PlayerPrefs.GetInt("LastProgressIndex");
            SimulateProgress(savedIndex);
        }
    }

    public void ShowPopUp(int index)
    {
        if (index >= 0 && index < popUpPanels.Length)
        {
            popUpPanels[index].SetActive(true);
        }
    }

    public void HidePopUp(int index)
    {
        if (index >= 0 && index < popUpPanels.Length)
        {
            popUpPanels[index].SetActive(false);
        }
    }

    public void ShowProgress(int index)
    {
        if (index >= 0 && index < schatkisten.Length)
        {
            // Enable zandhopen before this index
            for (int i = 0; i < index && i < zandhopen.Length; i++)
            {
                zandhopen[i].SetActive(true);
            }

            // Disable all previous schatkisten
            for (int i = 0; i < index; i++)
            {
                schatkisten[i].SetActive(false);
            }

            // Enable the current schatkist
            schatkisten[index].SetActive(true);

            // Disable all buttons up to and including this one
            for (int i = 0; i <= index && i < progressButtons.Length; i++)
            {
                if (progressButtons[i] != null)
                {
                    progressButtons[i].interactable = false;
                }
            }

            // Always save progress
            PlayerPrefs.SetInt("LastProgressIndex", index);
            PlayerPrefs.Save();
        }
    }

    private void SimulateProgress(int index)
    {
        if (index >= 0 && index < schatkisten.Length)
        {
            for (int i = 0; i < index && i < zandhopen.Length; i++)
            {
                zandhopen[i].SetActive(true);
            }

            for (int i = 0; i < index; i++)
            {
                schatkisten[i].SetActive(false);
            }

            schatkisten[index].SetActive(true);

            for (int i = 0; i <= index && i < progressButtons.Length; i++)
            {
                if (progressButtons[i] != null)
                {
                    progressButtons[i].interactable = false;
                }
            }
        }
    }
}
