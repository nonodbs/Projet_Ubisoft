using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtenergy;
    public Image INpercent;
    public MapManager mapManager;
    public GameObject PauseCanvas;
    public GameObject LostCanvas;
    public TextMeshProUGUI scoreText;
    public bool ispause = false;

    private float score;
    private float maxScore = 100;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        txtenergy.text = "Energy: " + mapManager.GetEnergy().ToString();
        score = mapManager.GetEvolutionPercentage();
        INpercent.fillAmount = (score / maxScore);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseCanvas != null)
            {
                PauseCanvas.SetActive(!PauseCanvas.activeSelf);
                ispause = true;
            }
        }
    }

    public void Replay()
    {
        if (PauseCanvas != null)
        {
            PauseCanvas.SetActive(!PauseCanvas.activeSelf);
            ispause = false;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
        ispause = false;
    }

    public void lost()
    {
        if (LostCanvas != null)
        {
            LostCanvas.SetActive(true);
            scoreText.text = "Score: " + mapManager.GetEvolutionPercentage().ToString() + "%";
            ispause = true;
        }
    }
}
