using System.Collections;
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

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Tile> tilesToPlace = new List<Tile>();
    public int width = 10;
    public int height = 10;
    
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
                //TileBase randomTile = GetRandomTile();
                //tilemap.SetTile(position, randomTile);
                tilemap.SetTile(position, tilesToPlace[0].tile);
            }
        }
    }

    TileBase GetRandomTile()
    {
        if (tilesToPlace.Count == 0) return null;

        int randomIndex = Random.Range(0, tilesToPlace.Count);
        return tilesToPlace[randomIndex].tile;
    }

}
