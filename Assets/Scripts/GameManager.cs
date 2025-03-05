using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CellStateSpace;


[System.Serializable]
public class DifficultySettings
{
    public string difficultyName;
    public int baseEnergy;
}

public class GameManager : MonoBehaviour
{
    // gestion des paramètres de la partie
    
    [SerializeField]
    public TMP_InputField inputHeight;
    [SerializeField]
    public TMP_InputField inputWidth;
    [SerializeField]
    public TMP_Dropdown difficultyDropdown;

    [SerializeField]
    public List<DifficultySettings> difficultySettingsList;

    private int height;
    private int width;
    private string difficulty;

    public GameObject rulesMenu;
    public Button rulesButton;
    public Button closeButton;

    public void StartLevel()
    {
        rulesMenu.SetActive(false);
        rulesButton.onClick.AddListener(OpenRulesMenu);
        closeButton.onClick.AddListener(CloseRulesMenu);
        if (!int.TryParse(inputHeight.text, out height) || height < 20 || height > 100)
        {
            height = 20;
        }

        if (!int.TryParse(inputWidth.text, out width) || width < 20 || width > 100)
        {
            width = 20;
        }

        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("Width", width);
        difficulty = difficultyDropdown.options[difficultyDropdown.value].text;
        PlayerPrefs.SetString("Difficulty", difficulty);

        int baseEnergy = GetBaseEnergyForDifficulty(difficulty);
        PlayerPrefs.SetInt("Energy", baseEnergy);
        SceneManager.LoadScene("MainLevel");
    }

    private int GetBaseEnergyForDifficulty(string difficulty)
    {
        foreach (var set in difficultySettingsList)
        {
            if (set.difficultyName == difficulty)
            {
                return set.baseEnergy;
            }
        }
        return 0; // Valeur par défaut si la difficulté n'est pas trouvée
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnHeightChanged()
    {
        if (int.TryParse(inputHeight.text, out int newheight))
        {
            height = Mathf.Clamp(newheight, 20, 100);
        }
        else
        {
            Debug.LogWarning("Valeur invalide saisie !");
        }
    }

    public void OnWightChanged()
    {
        if (int.TryParse(inputWidth.text, out int newwidth))
        {
            width = Mathf.Clamp(newwidth, 20, 100);
        }
        else
        {
            Debug.LogWarning("Valeur invalide saisie !");
        }
    }

    public void OpenRulesMenu()
    {
        rulesMenu.SetActive(true);
    }

    public void CloseRulesMenu()
    {
        rulesMenu.SetActive(false);
    }

    public int Getheight()
    {
        return height;
    }

    public int Getwidth()
    {
        return width;
    }
}
