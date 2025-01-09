using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MySceneManager : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField Inputheight;
    [SerializeField]
    public TMP_InputField Inputwidth;
    [SerializeField]
    public TMP_Dropdown difficultyDropdown;

    private int height;
    private int width;
    private string difficulty;

    public void StartLevel()
    {
        if (!int.TryParse(Inputheight.text, out height) || height < 10 || height > 100)
        {
            height = 10;
        }

        if (!int.TryParse(Inputwidth.text, out width) || width < 10 || width > 100)
        {
            width = 10;
        }

        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("Width", width);
        difficulty = difficultyDropdown.options[difficultyDropdown.value].text;
        PlayerPrefs.SetString("difficulty", difficulty);
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
        if (int.TryParse(Inputheight.text, out int newheight))
        {
            height = Mathf.Clamp(newheight, 10, 100);
        }
        else
        {
            Debug.LogWarning("Valeur invalide saisie !");
        }
    }

    public void OnWightChanged()
    {
        if (int.TryParse(Inputwidth.text, out int newwidth))
        {
            width = Mathf.Clamp(newwidth, 10, 100);
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

    public string GetDifficulty()
    {
        return difficulty;
    }
}
