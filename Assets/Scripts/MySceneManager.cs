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
        PlayerPrefs.SetInt("Height", int.Parse(Inputheight.text));
        PlayerPrefs.SetInt("Width", int.Parse(Inputwidth.text));
        difficulty = difficultyDropdown.options[difficultyDropdown.value].text;
        PlayerPrefs.SetString("difficulty", difficulty);
        SceneManager.LoadScene("MainLevel");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
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
