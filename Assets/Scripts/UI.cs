using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //gestion de l'interface utilisateur (UI) (énergie, % d'évolution, pause, fin de partie)
    public TextMeshProUGUI txtenergy;
    public Image INpercent;
    //public MapManager mapManager;
    //public GameManager gamemanager;
    public GameObject PauseCanvas;
    public GameObject LostCanvas;
    public GameObject WinCanvas;
    public GameObject AllespeceCanvas;
    public TextMeshProUGUI scoreText;
    public bool ispause = false;
    public bool haveall = false;

    private float score;
    private float maxScore = 100;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        txtenergy.text = ": " + InfoManager.Instance.GetEnergy().ToString();
        score = InfoManager.Instance.GetEvolutionPercentage();
        INpercent.fillAmount = (score / maxScore);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseCanvas != null)
            {
                PauseCanvas.SetActive(!PauseCanvas.activeSelf);
                ispause = true;
            }
        }
        if(InfoManager.Instance.animalInGame.Count == InfoManager.Instance.animalPrefab.Count && !haveall)
        {
            if (AllespeceCanvas != null)
            {
                AllespeceCanvas.SetActive(!AllespeceCanvas.activeSelf);
                ispause = true;
                haveall = true;
            }
        }
        else if (score >= maxScore)
        {
            win();
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

    public void continue_game()
    {
        if (AllespeceCanvas != null)
        {
            AllespeceCanvas.SetActive(!AllespeceCanvas.activeSelf);
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
        if(ispause)
        {
            return;
        }
        if (LostCanvas != null)
        {
            LostCanvas.SetActive(true);
            int scoreint = Mathf.RoundToInt(InfoManager.Instance.GetEvolutionPercentage());
            scoreText.text = "Score: " + scoreint.ToString() + "%";
            ispause = true;
        }
    }

    public void win()
    {
        if (WinCanvas != null)
        {
            WinCanvas.SetActive(true);
            int scoreint = Mathf.RoundToInt(InfoManager.Instance.GetEvolutionPercentage());
            scoreText.text = "Score: " + scoreint.ToString() + "%";
            ispause = true;
        }
    }
    
}
