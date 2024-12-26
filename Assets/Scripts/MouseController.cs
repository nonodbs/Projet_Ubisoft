using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public GameObject overlayTilePrefab;
    public Transform overlayContainer;
    public Tilemap tilemap;
    public MapManager mapManager;

    public float ClickDuration = 2;
    bool clicking = false;
    float totalDownTime = 0;

    private List<Vector3Int> selectedTiles = new List<Vector3Int>();

    void Update()
    {
        if (Cursor.visible)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPosition);
            TileBase tile = tilemap.GetTile(tilePos);

            if (tile != null)
            {
                cursor.transform.position = tilemap.GetCellCenterWorld(tilePos);
            }

            if (Input.GetMouseButtonDown(0) && tile != null)
            {
                OnTileClicked(tilePos);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                clicking = true;
                totalDownTime= 0;
            }

            if (clicking && Input.GetKey(KeyCode.Space))
            {
                totalDownTime += Time.deltaTime;
            }

            if (clicking && Input.GetKeyUp(KeyCode.Space))
            {
                clicking = false;
                if (totalDownTime >= ClickDuration)
                {
                    ChangeSelectedTiles();
                    RemoveAllOverlays();
                }
                else
                {
                    EvolveSelectedTiles();
                    RemoveAllOverlays();
                }
            }
        }
    }

    void OnTileClicked(Vector3Int tilePos)
    {
        string overlayName = $"Overlay_{tilePos.x}_{tilePos.y}";
        Transform existingOverlay = overlayContainer.Find(overlayName);

        if (existingOverlay == null)
        {
            if (overlayTilePrefab != null)
            {
                GameObject overlayTile = Instantiate(overlayTilePrefab, overlayContainer);
                overlayTile.name = overlayName;
                Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(tilePos);
                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 0.1f);
            }

            selectedTiles.Add(tilePos);
        }
        else
        {
            selectedTiles.Remove(tilePos);
            Destroy(existingOverlay.gameObject);
        }
    }

    void EvolveSelectedTiles()
    {
        foreach (var tilePos in selectedTiles)
        {
            mapManager.EvolveTile(tilePos);
        }

        selectedTiles.Clear();
    }

    void ChangeSelectedTiles()
    {
        foreach (var tilePos in selectedTiles)
        {
            mapManager.SetTileToWater(tilePos);
        }

        selectedTiles.Clear();
    }

    void RemoveAllOverlays()
    {
        foreach (Transform child in overlayContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
