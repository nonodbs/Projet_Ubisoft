using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CellStateSpace;

public class GameManager : MonoBehaviour
{
    // gestion des paramètres de la partie
    
    [SerializeField]
    public TMP_InputField inputHeight;
    [SerializeField]
    public TMP_InputField inputWidth;
    [SerializeField]
    public TMP_Dropdown difficultyDropdown;

    private int height;
    private int width;
    private string difficulty;

    public void StartLevel()
    {
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
        if (difficulty == "peaceful")
        {
            PlayerPrefs.SetInt("Energy", 0);
        }
        else if (difficulty == "easy")
        {
            PlayerPrefs.SetInt("Energy", 120);
        }
        else if (difficulty == "normal")
        {
            PlayerPrefs.SetInt("Energy", 80);
        }
        else if (difficulty == "hard")
        {
            PlayerPrefs.SetInt("Energy", 50);
        }
        SceneManager.LoadScene("MainLevel");
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

    public int Getheight()
    {
        return height;
    }

    public int Getwidth()
    {
        return width;
    }
}
