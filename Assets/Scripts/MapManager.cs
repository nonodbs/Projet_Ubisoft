using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public class MapManager : MonoBehaviour
{
    // gestion de la map (generation)
    public TilesManager tilesManager;
    public float noiseScale = 0.1f;
    public int seed;

    [Header("Height Thresholds")]
    public float waterThreshold = 0.1f;
    public float swampThreshold = 0.2f;
    public float landThreshold = 0.6f;
    public float forestThreshold = 0.7f;

    [Header("River Generation")]
    public int minRivers = 1;
    public int maxRivers = 3;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // Générer la map
        int height = PlayerPrefs.GetInt("Height", 20);
        int width = PlayerPrefs.GetInt("Width", 20);

        float[,] noiseMap = GenerateNoiseMap(width, height, noiseScale, seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                float heightValue = noiseMap[x, y];

                if (heightValue < waterThreshold)
                {
                    tilesManager.SetTileToWater(position);
                }
                else if (heightValue < swampThreshold)
                {
                    tilesManager.SetTileToSwamp(position);
                }
                else if (heightValue < landThreshold)
                {
                    tilesManager.SetTileToLand(position);
                }
                else if (heightValue < forestThreshold)
                {
                    tilesManager.SetTileToForest(position);
                }
                else
                {
                    tilesManager.SetTileToTree(position);
                }
            }
        }

        GenerateRivers();
    }

    float[,] GenerateNoiseMap(int width, int height, float scale, int seed)
    {
        // Générer une map de bruit de Perlin
        float[,] noiseMap = new float[width, height];
        System.Random prng = new System.Random(seed);
        float offsetX = prng.Next(-100000, 100000);
        float offsetY = prng.Next(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sampleX = x * scale + offsetX;
                float sampleY = y * scale + offsetY;
                float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }

    void GenerateRivers()
    {
        int height = PlayerPrefs.GetInt("Height", 20);
        int width = PlayerPrefs.GetInt("Width", 20);

        // Générer des rivières
        int numberOfRivers = Random.Range(minRivers, maxRivers);
        for (int i = 0; i < numberOfRivers; i++)
        {
            Vector3Int start = new Vector3Int(Random.Range(0, width), Random.Range(0, height), 0);
            Vector3Int end = new Vector3Int(Random.Range(0, width), Random.Range(0, height), 0);
            List<Vector3Int> riverPath = GenerateRiverPath(start, end);

            foreach (Vector3Int position in riverPath)
            {
                tilesManager.SetTileToWater(position);
                SetSwampAroundWater(position);
            }
        }
    }

    List<Vector3Int> GenerateRiverPath(Vector3Int start, Vector3Int end)
    {
        // Générer un chemin de rivière entre deux points
        List<Vector3Int> path = new List<Vector3Int>();

        int x0 = start.x;
        int y0 = start.y;
        int x1 = end.x;
        int y1 = end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            path.Add(new Vector3Int(x0, y0, 0));

            if (x0 == x1 && y0 == y1) break;
            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        List<Vector3Int> expandedPath = new List<Vector3Int>();
        foreach (Vector3Int pos in path)
        {
            for (int dxx = -1; dxx <= 1; dxx++)
            {
                for (int dyy = -1; dyy <= 1; dyy++)
                {
                    Vector3Int expandedPos = new Vector3Int(pos.x + dxx, pos.y + dyy, 0);
                    if (!expandedPath.Contains(expandedPos))
                    {
                        expandedPath.Add(expandedPos);
                    }
                }
            }
        }

        return expandedPath;
    }

    void SetSwampAroundWater(Vector3Int position)
    {
        // Mettre des marais autour de l'eau

        foreach (Vector3Int direction in InfoManager.directions)
        {
            Vector3Int neighborPos = position + direction;
            if ((!InfoManager.Instance.mapState.ContainsKey(neighborPos) || InfoManager.Instance.mapState[neighborPos].type != TileType.Water))
            {
                tilesManager.SetTileToSwamp(neighborPos);
            }
        }
    }
}