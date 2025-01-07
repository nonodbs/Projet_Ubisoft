using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI txtenergy;
    public Image INpercent;
    public MapManager mapManager;

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
    }
}
