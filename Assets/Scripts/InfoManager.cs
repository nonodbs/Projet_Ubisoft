using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public class InfoManager : MonoBehaviour
{
    // gestion des informations de la partie (énergie, évolutions, difficulté, tilemap)
    public static InfoManager Instance { get; private set; }

    public List<Evolutions> evolutionStages;
    public Dictionary<Vector3Int, CellState> mapState = new Dictionary<Vector3Int, CellState>();

    public Tilemap tilemap;
    public UIManager uimanager;

    public List<GameObject> animalInGame;

    public List<GameObject> animalPrefab;

    public static Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(-1, -1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 1, 0)
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetDifficulty()
    {
        return PlayerPrefs.GetString("difficulty");
    }

    public int GetEnergy()
    {
        return PlayerPrefs.GetInt("Energy", 0);
    }

    public void SetEnergy(int energy)
    {
        PlayerPrefs.SetInt("Energy", energy);
    }

    public bool Ispeaceful()
    {
        return PlayerPrefs.GetString("Difficulty", "peaceful") == "peaceful";
    }

    public float GetEvolutionInterval()
    {
        string difficulty = GetDifficulty();
        switch (difficulty)
        {
            case "easy":
                return 30f;
            case "medium":
                return 45f;
            case "hard":
                return 60f;
            default:
                return 30f; // Intervalle par défaut
        }
    }

    public void AddEnergy(int amount)
    {
        if (!Ispeaceful())
        {
            int currentEnergy = GetEnergy();
            SetEnergy(currentEnergy + amount);
        }
    }

    public void SubEnergy(int amount)
    {
        if (!Ispeaceful())
        {
            int currentEnergy = GetEnergy();
            SetEnergy(currentEnergy - amount);
            if (GetEnergy() <= 0)
            {
                //mort du joueur
                uimanager.lost(); 
            }
        }
    }
    public float GetEvolutionPercentage()
    {
        //Calcule le % de la map en fonction des évolutions
        int totalStages = 0;
        int totalProgress = 0;

        foreach (var entry in InfoManager.Instance.mapState)
        {
            Vector3Int position = entry.Key;
            CellState state = entry.Value;
            Evolutions evolutions = InfoManager.Instance.evolutionStages.Find(e => e.type == state.type);

            if (evolutions != null || state.type == TileType.Water)
            {
                totalStages += evolutions.stages.Count - 1;
                totalProgress += state.evol;
            }
        }
        if (totalStages == 0) return 0;
        float evolutionPercentage = (float)totalProgress / totalStages * 100f;
        return evolutionPercentage;
    }
}