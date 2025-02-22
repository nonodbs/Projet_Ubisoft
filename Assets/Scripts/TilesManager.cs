using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CellStateSpace;

public enum TileType
{
    Land,
    Swamp,
    Water,
    Tree,
    Forest
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

public class TilesManager : MonoBehaviour
{
    //gestion des tiles (fonction d'évolution, getter de tile avec des paramètres...)
    public bool PosInMap(Vector3Int pos)
    {
        //Vérifie si la position est dans la map
        int height = PlayerPrefs.GetInt("Height", 20);
        int width = PlayerPrefs.GetInt("Width", 20);
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public TileBase GetTileFromEvolution(TileType type, int evol)
    {
        //Recupère l'évolution de la tile
        Evolutions evolutions = InfoManager.Instance.evolutionStages.Find(e => e.type == type);
        if (evolutions != null && evol < evolutions.stages.Count)
        {
            return evolutions.stages[evol];
        }
        return null;
    }

    public void SetTileToTree(Vector3Int position)
    {
        //Change la tile en arbre
        if (!PosInMap(position))
            return;
        TileBase treeTile = GetTileFromEvolution(TileType.Tree, 0);
        if (treeTile != null)
        {
            InfoManager.Instance.tilemap.SetTile(position, treeTile);
            InfoManager.Instance.mapState[position] = new CellState { type = TileType.Tree, evol = 0 };
        }
    }

    public void SetTileToLand(Vector3Int position)
    {
        //Change la tile en herbe
        if (!PosInMap(position))
            return;
        TileBase landTile = GetTileFromEvolution(TileType.Land, 0);
        if (landTile != null)
        {
            InfoManager.Instance.tilemap.SetTile(position, landTile);
            InfoManager.Instance.mapState[position] = new CellState { type = TileType.Land, evol = 0 };
        }
    }

    public void SetTileToWater(Vector3Int position)
    {
        //Change la tile en eau
        if (!PosInMap(position))
            return;
        TileBase waterTile = GetTileFromEvolution(TileType.Water, 0);
        if (waterTile != null)
        {
            InfoManager.Instance.tilemap.SetTile(position, waterTile);
            InfoManager.Instance.mapState[position] = new CellState { type = TileType.Water, evol = 0 };
        }
    }

    public void SetTileToSwamp(Vector3Int position)
    {
        //Change la tile en marais
        if (!PosInMap(position))
            return;
        TileBase swampTile = GetTileFromEvolution(TileType.Swamp, 0);
        if (swampTile != null)
        {
            InfoManager.Instance.tilemap.SetTile(position, swampTile);
            InfoManager.Instance.mapState[position] = new CellState { type = TileType.Swamp, evol = 0 };
        }
    }

    public void SetTileToForest(Vector3Int position)
    {
        //Change la tile en forêt
        if (!PosInMap(position))
            return;
        TileBase forestTile = GetTileFromEvolution(TileType.Forest, 0);
        if (forestTile != null)
        {
            InfoManager.Instance.tilemap.SetTile(position, forestTile);
            InfoManager.Instance.mapState[position] = new CellState { type = TileType.Forest, evol = 0 };
        }
    }

}
