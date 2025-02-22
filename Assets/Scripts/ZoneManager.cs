using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public enum ZoneType
{
    Meadow, //plaine, bunny
    Field, //champ, deer
    Forest, //wolf
    Swamp, //marrais, boar
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

    // gestion des zones dynamiques (creation, suppression, mise à jour) pour l'apparition des animaux
    public AnimalsManager animalsManager;
    private Dictionary<Vector3Int, DynamicZone> tileToZoneMap = new Dictionary<Vector3Int, DynamicZone>();
    public List<DynamicZone> zones = new List<DynamicZone>();

    private static readonly Vector3Int[] directions = {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(-1, -1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, 1, 0)
    };

    public List<DynamicZone> GetZones()
    {
        return zones;
    }

    public ZoneType GetZoneType(CellState state)
    {
        if (state.type == TileType.Swamp && state.evol >= 3)
        {
            return ZoneType.Swamp; //boar 
        }
        if (state.type == TileType.Land && state.evol == 2)
        {
            return ZoneType.Meadow; //bunny
        }
        if (state.type == TileType.Land && state.evol == 3)
        {
            return ZoneType.Field; //deer
        }
        if((state.type == TileType.Tree || state.type == TileType.Forest) && state.evol >= 2)
        {
            return ZoneType.Forest; //wolf
        }
        else
            return ZoneType.None_Zone;
    }

    public int GetAnimalType(ZoneType type)
    {
        if (type == ZoneType.Meadow)
        {
            return 0; //bunny
        }
        if (type == ZoneType.Field)
        {
            return 1; //deer
        }
        if (type == ZoneType.Forest)
        {
            return 2; //wolf
        }
        if (type == ZoneType.Swamp)
        {
            return 3; //boar
        }
        else
            return 4; //bird
    }

    public DynamicZone GetZoneAtPosition(Vector3Int pos)
    {
        tileToZoneMap.TryGetValue(pos, out DynamicZone zone);
        return zone;
    }


      public void CreateZoneFromTiles(ZoneType zoneType, TileType tileType, List<Vector3Int> positions)
    {
        DynamicZone newZone = new DynamicZone { name = zoneType, type = tileType, positions = positions, nbanimals = 0 };
        zones.Add(newZone);
        foreach (var pos in positions)
        {
            tileToZoneMap[pos] = newZone;
        }
        //Debug.Log($"Zone created: {zoneType} with {positions.Count} tiles.");
        AddAnimals(newZone);
    }

    public void DeleteZone(Vector3Int pos)
    {
        DynamicZone zoneToDelete = GetZoneAtPosition(pos);
        if (zoneToDelete != null)
        {
            foreach (var position in zoneToDelete.positions)
            {
                tileToZoneMap.Remove(position);
            }
            zones.Remove(zoneToDelete);
            //Debug.Log($"Zone deleted at position: {pos}");
        }
    }

    public void UpdateZone(Vector3Int pos, CellState newTile)
    {
        if (!InfoManager.Instance.mapState.ContainsKey(pos))
        {
            return;
        }

        InfoManager.Instance.mapState[pos] = newTile;
        DynamicZone currentZone = GetZoneAtPosition(pos);

        if (currentZone != null)
        {
            currentZone.positions.Remove(pos);
            tileToZoneMap.Remove(pos);
            if (currentZone.positions.Count == 0)
            {
                zones.Remove(currentZone);
                //Debug.Log($"Zone removed: {currentZone.name} at position: {pos}");
            }
            else if (currentZone.positions.Count < 6)
            {
                RemoveAnimals(currentZone);
            }
        }

        List<DynamicZone> adjacentZones = new List<DynamicZone>();
        foreach (var direction in directions)
        {
            Vector3Int adjacentPos = pos + direction;
            if (!InfoManager.Instance.mapState.ContainsKey(adjacentPos))
            {
                continue;
            }
            CellState adj = InfoManager.Instance.mapState[adjacentPos];
            DynamicZone adjacentZone = GetZoneAtPosition(adjacentPos);
            if (adjacentZone != null && adj.type == newTile.type
                && !adjacentZones.Contains(adjacentZone) && adj.evol == newTile.evol)
            {
                adjacentZones.Add(adjacentZone);
            }
        }

        if (adjacentZones.Count == 0)
        {
            ZoneType newZoneType = GetZoneType(InfoManager.Instance.mapState[pos]);
            if (newZoneType == ZoneType.None_Zone)
            {
                return;
            }
            CreateZoneFromTiles(newZoneType, newTile.type, new List<Vector3Int> { pos });
        }
        else
        {
            DynamicZone mainZone = adjacentZones[0];
            mainZone.positions.Add(pos);
            tileToZoneMap[pos] = mainZone;
            //Debug.Log($"Tile at {pos} added to existing zone: {mainZone.name}");
            AddAnimals(mainZone);

            for (int i = 1; i < adjacentZones.Count; i++)
            {
                DynamicZone zoneToMerge = adjacentZones[i];
                foreach (var position in zoneToMerge.positions)
                {
                    mainZone.positions.Add(position);
                    tileToZoneMap[position] = mainZone;
                }
                zones.Remove(zoneToMerge);
                //Debug.Log($"Zones merged: {mainZone.name} absorbed {zoneToMerge.name}");
            }

            // Mettre à jour le type de la zone principale
            ZoneType updatedZoneType = GetZoneType(InfoManager.Instance.mapState[pos]);
            if (mainZone.name != updatedZoneType && updatedZoneType != ZoneType.None_Zone)
            {
                mainZone.name = updatedZoneType;
                //Debug.Log($"Zone type updated to: {updatedZoneType} for zone at position: {pos}");
            }
        }
    }

    public void AddAnimals(DynamicZone zone)
    {
        if (zone.positions.Count % 8 == 0 && zone.nbanimals < 6)
        {
            animalsManager.SpawnAnimalInZone(zone, GetAnimalType(zone.name));
            zone.nbanimals += 1;
            //Debug.Log($"Animal spawned in zone: {zone.name}");
        }
    }

    private void RemoveAnimals(DynamicZone zone)
    {
        if (zone.nbanimals > 0)
        {
            animalsManager.DespawnAnimals(zone);
            zone.nbanimals = 0;
            //Debug.Log($"Animals removed from zone: {zone.name}");
        }
    }
}
