using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;
using UnityEngine.SceneManagement;

public enum TileType
{
    Land,
    Swamp,
    Water
    //Stone
}

[System.Serializable]
public class Tile
{
    public TileBase tile;
    public TileType type;
}

[System.Serializable]
public class Evolutions
{
    public TileType type;
    public List<TileBase> stages;
}

namespace CellStateSpace
{
    [System.Serializable]
    public class CellState
    {
        public TileType type;
        public int evol;
    }
}

public class MapManager : MonoBehaviour
{
    public ZoneManager zoneManager;
    public GameObject menuPanel;
    //public MySceneManager mySceneManager;
    public Tilemap tilemap;
    public List<Evolutions> evolutionStages;

    //public int height = 10;
    //public int width = 10;

    private string difficulty;
    private bool peaceful = false;
    private int Energy = 0;

    public bool ispause = false;

    //Dictionnaire pour stoker l'état de chaque tile (type, évolution)
    private Dictionary<Vector3Int, CellState> mapState = new Dictionary<Vector3Int, CellState>();

    void Start()
    {
        difficulty = PlayerPrefs.GetString("difficulty", "peaceful");
        GenerateMap();
        if (difficulty == "peaceful")
        {
            Energy = 0;
            peaceful = true;
        }
        else if (difficulty == "easy")
        {
            Energy = 120;
        }
        else if (difficulty == "normal")
        {
            Energy = 80;
        }
        else if (difficulty == "hard")
        {
            Energy = 50;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(!menuPanel.activeSelf);
                ispause = true;
            }
        }
    }

    public void Replay()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            ispause = false;
        }
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
        ispause = false;
    }

    void GenerateMap()
    {
        //Genere une map (pour le moment uniquement en terre)
        int height = PlayerPrefs.GetInt("Height", 10);
        int width = PlayerPrefs.GetInt("Width", 10);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileType randomType = GetRandomTileType();
                TileBase initialTile = GetTileFromEvolution(randomType, 0);
                if (initialTile != null)
                {
                    tilemap.SetTile(position, initialTile);
                    mapState[position] = new CellState { type = randomType, evol = 0 };
                }
            }
        }
    }

    TileType GetRandomTileType()
    {
        //return evolutionStages[Random.Range(0, evolutionStages.Count)].type;
        return TileType.Land;
    }

    TileBase GetTileFromEvolution(TileType type, int evol)
    {
        //Recupère l'évolution de la tile
        Evolutions evolutions = evolutionStages.Find(e => e.type == type);
        if (evolutions != null && evol < evolutions.stages.Count)
        {
            return evolutions.stages[evol];
        }
        return null;
    }

    public void SetTileToWater(Vector3Int pos)
    {
        //mets la tile à la position en eau
        if (mapState.ContainsKey(pos))
        {
            CellState state = mapState[pos];
            TileBase wat = GetTileFromEvolution(TileType.Water, 0);
            if (wat != null)
            {
                state.evol = 0;
                state.type = TileType.Water;
                tilemap.SetTile(pos, wat);
                SubEnergy(5);
            }
        }

        //Change le type des tiles adjacente à l'eau en swamp
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = pos + direction;
            if (mapState.ContainsKey(neighborPos))
            {
                CellState neighborState = mapState[neighborPos];
                TileBase swampTile = GetTileFromEvolution(TileType.Swamp, 0);
                if (swampTile != null && neighborState.type != TileType.Water)
                {
                    neighborState.evol = 0;
                    neighborState.type = TileType.Swamp;
                    tilemap.SetTile(neighborPos, swampTile);
                }
            }
        }
        zoneManager.DeleteZone(pos);
        zoneManager.CreateZone(pos, mapState);
    }

    public void EvolveTile(Vector3Int pos)
    {
        //Faot évolué la tile à la position
        if (mapState.ContainsKey(pos))
        {
            CellState state = mapState[pos];
            int nextEvol = state.evol + 1;
            TileBase nextTile = GetTileFromEvolution(state.type, nextEvol);
            if (nextTile != null)
            {
                state.evol = nextEvol;
                tilemap.SetTile(pos, nextTile);
                SubEnergy(state.evol);
            }
        }
        zoneManager.DeleteZone(pos);
        zoneManager.CreateZone(pos, mapState);
    }

    public int GetEnergy()
    {
        return Energy;
    }

    public void AddEnergy(int adding)
    {
        Energy += adding;
    }

    void SubEnergy(int amount)
    {
        if (peaceful == false)
        {
            Energy -= amount;
            if (Energy <= 0)
            {
                //mort du joueur
            }
        }
    }

    public float GetEvolutionPercentage()
    {
        //Calcule le % de la map en fonction des évolutions
        int totalStages = 0;
        int totalProgress = 0;

        foreach (var entry in mapState)
        {
            Vector3Int position = entry.Key;
            CellState state = entry.Value;
            Evolutions evolutions = evolutionStages.Find(e => e.type == state.type);

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