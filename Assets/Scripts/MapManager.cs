using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Grass,
    Water,
    Stone
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

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Evolutions> evolutionStages;

    public int width = 10;
    public int height = 10;

    private Dictionary<Vector3Int, CellState> mapState = new Dictionary<Vector3Int, CellState>();

    private class CellState
    {
        public TileType type;
        public int evol;
    }

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
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
        return evolutionStages[Random.Range(0, evolutionStages.Count)].type;
    }

    TileBase GetTileFromEvolution(TileType type, int evol)
    {
        Evolutions evolutions = evolutionStages.Find(e => e.type == type);
        if (evolutions != null && evol < evolutions.stages.Count)
        {
            return evolutions.stages[evol];
        }
        return null;
    }

    public void SetTileToWater(Vector3Int pos)
    {
        if (mapState.ContainsKey(pos))
        {
            CellState state = mapState[pos];
            TileBase wat = GetTileFromEvolution(TileType.Water, 0);
            if (wat != null)
            {
                state.evol = 0;
                state.type = TileType.Water;
                tilemap.SetTile(pos, wat);
            }
        }
    }

    public void EvolveTile(Vector3Int pos)
    {
        if (mapState.ContainsKey(pos))
        {
            CellState state = mapState[pos];
            int nextEvol = state.evol + 1;
            TileBase nextTile = GetTileFromEvolution(state.type, nextEvol);
            if (nextTile != null)
            {
                state.evol = nextEvol;
                tilemap.SetTile(pos, nextTile);
            }
        }
    }
}
