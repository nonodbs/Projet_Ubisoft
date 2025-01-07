using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public enum ZoneType
{
    Meadow,
    Field,
    Pond,
    Forest,
    None_Zone
    //Stone
}

public class DynamicZone
{
    public ZoneType name;
    public TileType type;
    public List<Vector3Int> positions;
    public int nbanimals;
}

public class ZoneManager : MonoBehaviour
{
    private List<DynamicZone> zones = new List<DynamicZone>();
    public AnimalsManager animalsManager;

    public ZoneType GetZoneType(CellState state)
    {
        if (state.type == TileType.Water && state.evol == 0)
        {
            return ZoneType.Pond;
        }
        if (state.type == TileType.Land && state.evol == 3)
        {
            return ZoneType.Meadow;
        }
        if (state.type == TileType.Land && state.evol == 4)
        {
            return ZoneType.Field;
        }
        else
            return ZoneType.None_Zone;
    }

    public int GetAnimalType(ZoneType type)
    {
        if (type == ZoneType.Pond)
        {
            return 3; //fish
        }
        if (type == ZoneType.Meadow)
        {
            return 0; //bunny
        }
        if (type == ZoneType.Field)
        {
            return 1; //duck
        }
        else
            return -1;
    }

    public DynamicZone GetZoneAtPosition(Vector3Int pos)
    {
        foreach (var zone in zones)
        {
            if ((zone.positions.Contains(pos))) return zone;
        }
        return null;
    }


    public void CreateZone(Vector3Int pos, Dictionary<Vector3Int, CellState> mapState)
    {
        if (!mapState.ContainsKey(pos)) return;

        if ((GetZoneType(mapState[pos]) == ZoneType.None_Zone)) return;

        CellState state = mapState[pos];
        DynamicZone existingZone = GetZoneAtPosition(pos);
        if (existingZone != null)
        {
            return;
        }

        DynamicZone adjacentZone = GetAdjacentZone(pos);
        if (adjacentZone != null)
        {
            adjacentZone.positions.Add(pos);
            AddAnimals(adjacentZone);
            return;
        }

        List<Vector3Int> neighbors = new List<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> explore = new Queue<Vector3Int>();

        explore.Enqueue(pos);

        while (explore.Count > 0)
        {
            Vector3Int current = explore.Dequeue();
            if (visited.Contains(current)) continue;

            visited.Add(current);

            if (mapState.ContainsKey(current))
            {
                CellState currentState = mapState[current];
                if (currentState.type == state.type && currentState.evol == state.evol)
                {
                    neighbors.Add(current);
                    Vector3Int[] directions = new Vector3Int[] 
                    {
                        new Vector3Int(1, 0, 0),
                        new Vector3Int(-1, 0, 0),
                        new Vector3Int(0, 1, 0),
                        new Vector3Int(0, -1, 0)
                    };

                    foreach (var direction in directions)
                    {
                        Vector3Int neighborPos = current + direction;
                        if (!visited.Contains(neighborPos))
                        {
                            explore.Enqueue(neighborPos);
                        }
                    }
                }
            }
        }

        if (neighbors.Count > 6)
        {
            ZoneType zoneType = GetZoneType(state);
            CreateZoneFromTiles(zoneType, state.type, neighbors);       
        }
    }

    public DynamicZone GetAdjacentZone(Vector3Int pos)
    {
        Vector3Int[] directions = new Vector3Int[] 
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

        foreach (var zone in zones)
        {
            foreach (var position in zone.positions)
            {
                foreach (var direction in directions)
                {
                    Vector3Int adjacentPos = position + direction;
                    if (adjacentPos == pos)
                    {
                        return zone;
                    }
                }
            }
        }

        return null;
    }


    public void CreateZoneFromTiles(ZoneType zoneType, TileType tileType, List<Vector3Int> positions)
    {
        DynamicZone newZone = new DynamicZone { name = zoneType, type = tileType, positions = positions, nbanimals = 0 };
        zones.Add(newZone);
        AddAnimals(newZone);
    }

    public void DeleteZone(Vector3Int pos)
    {
        DynamicZone zoneToDelete = GetZoneAtPosition(pos);
        if (zoneToDelete != null)
        {
            zones.Remove(zoneToDelete);
        }
    }

    public void AddAnimals(DynamicZone zone)
    {
        if (zone.positions.Count % 5 == 0 && zone.nbanimals < 6)
        {
            animalsManager.SpawnAnimalInZone(zone, GetAnimalType(zone.name));
            zone.nbanimals += 1;
        }
    }
  
}
