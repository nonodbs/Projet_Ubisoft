using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public class EvolutionRule
{
    public TileType ActualType { get; set; }
    public int ActualEvol { get; set; }
    public TileType RequiredType { get; set; }
    public int RequiredEvol { get; set; }
    public int RequiredCount { get; set; }

    public TileType NewType { get; set; }
    public int NewEvol { get; set; }

    public EvolutionRule(TileType actualType,int actualEvol, TileType requiredType, int requiredEvol, int requiredCount, TileType newType, int newEvol)
    {
        ActualType = actualType;
        ActualEvol = actualEvol;
        RequiredType = requiredType;
        RequiredEvol = requiredEvol;
        RequiredCount = requiredCount;
        NewType = newType;
        NewEvol = newEvol;
    }
}

public class EvolutionManager : MonoBehaviour
{
    // gestioh des évolutions du monde
    public ZoneManager zoneManager;
    public TilesManager tilesManager;
    public GameManager gameManager;
    public float evolutionInterval = 45f; // Intervalle en secondes pour l'évolution du monde
    private float timer;
    private Dictionary<Vector3Int, CellState> stmapstate;
    private Tilemap stTilemap;

    private Vector3Int[] directions = new Vector3Int[]
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

    private List<EvolutionRule> evolutionRules = new List<EvolutionRule>();

    void Start()
    {
        stTilemap = InfoManager.Instance.tilemap;
        stmapstate = InfoManager.Instance.mapState;
        timer = evolutionInterval;

        // Initialisation des règles d'évolution

        // Land
        evolutionRules.Add(new EvolutionRule(TileType.Land, 0, TileType.Land, 2, 3, TileType.Land, 1));
        evolutionRules.Add(new EvolutionRule(TileType.Land, 1, TileType.Land, 3, 3, TileType.Land, 2));
        evolutionRules.Add(new EvolutionRule(TileType.Land, 2, TileType.Land, 4, 3, TileType.Land, 3));

        // Swamp
        evolutionRules.Add(new EvolutionRule(TileType.Swamp, 0, TileType.Water, 1, 2, TileType.Swamp, 1));
        evolutionRules.Add(new EvolutionRule(TileType.Swamp, 1, TileType.Swamp, 2, 3, TileType.Swamp, 2));

        // Tree
        evolutionRules.Add(new EvolutionRule(TileType.Tree, 0, TileType.Forest, 2, 5, TileType.Tree, 1));
        evolutionRules.Add(new EvolutionRule(TileType.Tree, 1, TileType.Forest, 3, 5, TileType.Tree, 2));

        // Forest
        evolutionRules.Add(new EvolutionRule(TileType.Forest, 0, TileType.Tree, 1, 3, TileType.Forest, 1));
        evolutionRules.Add(new EvolutionRule(TileType.Forest, 1, TileType.Tree, 2, 3, TileType.Forest, 2));
        evolutionRules.Add(new EvolutionRule(TileType.Forest, -1, TileType.Tree, 2, 5, TileType.Tree, 2));
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Debug.Log("Evolution du monde");
            EvolveWorld();
            timer = evolutionInterval;
        }
    }

    void EvolveWorld()
    {
        // Fait évoluer toutes les tuiles du monde
        List<Vector3Int> positionsToEvolve = new List<Vector3Int>(stmapstate.Keys);
        foreach (Vector3Int pos in positionsToEvolve)
        {
            foreach (var rule in evolutionRules)
            {
                ApplyRule(pos, rule);
            }
        }
    }

    private void ApplyRule(Vector3Int pos, EvolutionRule rule)
    {
        // Applique la règle d'évolution à la tuile
        if (!stmapstate.ContainsKey(pos))
        {
            return;
        }

        CellState state = stmapstate[pos];
        if (state.type != rule.ActualType || (rule.ActualEvol != -1 && state.evol != rule.ActualEvol))
        {
            return;
        }

        int count = 0;
        foreach (var direction in directions)
        {
            Vector3Int adjacentPos = pos + direction;
            if (stmapstate.ContainsKey(adjacentPos))
            {
                CellState adj = stmapstate[adjacentPos];
                if (adj.type == rule.RequiredType && (rule.RequiredEvol == -1 || adj.evol == rule.RequiredEvol))
                {
                    count++;
                }
            }
        }

        if (count >= rule.RequiredCount)
        {
            state.type = rule.NewType;
            state.evol = rule.NewEvol;
            stTilemap.SetTile(pos, tilesManager.GetTileFromEvolution(rule.NewType, rule.NewEvol));
            zoneManager.UpdateZone(pos, state);
            Debug.Log($"Tile at {pos} evolved to {rule.NewType} {rule.NewEvol}");
        }
    }

    public void EvolveTile(Vector3Int pos)
    {
        // Fait évoluer la tuile à la position
        if (stmapstate.ContainsKey(pos) && tilesManager.PosInMap(pos))
        {
            CellState state = stmapstate[pos];
            int nextEvol = state.evol + 1;
            TileBase nextTile = tilesManager.GetTileFromEvolution(state.type, nextEvol);
            if (nextTile != null)
            {
                state.evol = nextEvol;
                stTilemap.SetTile(pos, nextTile);
                InfoManager.Instance.SubEnergy(state.evol);
                zoneManager.UpdateZone(pos, state);
                if (state.type == TileType.Water)
                {
                    StartCoroutine(EvolveWater(pos));
                }
            }
        }
    }

    IEnumerator EvolveWater(Vector3Int startPos)
    {
        // Fait évoluer l'eau en fonction de la position de départ
        int radius = 2;
        Queue<Vector3Int> positionsToEvolve = new Queue<Vector3Int>();
        HashSet<Vector3Int> visitedPositions = new HashSet<Vector3Int>();
        positionsToEvolve.Enqueue(startPos);
        visitedPositions.Add(startPos);

        while (positionsToEvolve.Count > 0)
        {
            Vector3Int currentPos = positionsToEvolve.Dequeue();
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3Int newPos = currentPos + directions[i];
                if (stmapstate.ContainsKey(newPos) && stmapstate[newPos].type == TileType.Water && !visitedPositions.Contains(newPos))
                {
                    float distance = Vector3Int.Distance(startPos, newPos);
                    if (distance <= radius)
                    {
                        CellState state = stmapstate[newPos];
                        stTilemap.SetTile(newPos, tilesManager.GetTileFromEvolution(TileType.Water, 1));
                        zoneManager.UpdateZone(newPos, state);

                        positionsToEvolve.Enqueue(newPos);
                        visitedPositions.Add(newPos);

                        // Attendre entre 0.5 et 2 secondes avant de continuer
                        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
                    }
                }
            }
        }
    }

    public void ChangeTileToWater(Vector3Int pos)
    {
        // Mets la tile à la position en eau (action espace long)
        if (stmapstate.ContainsKey(pos))
        {
            CellState state = stmapstate[pos];
            state.type = TileType.Water;
            state.evol = 0;
            stTilemap.SetTile(pos, tilesManager.GetTileFromEvolution(TileType.Water, 0));
            zoneManager.UpdateZone(pos, state);
        }
    }
}
