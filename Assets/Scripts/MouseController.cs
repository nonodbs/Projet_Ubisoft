using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public GameObject overlayTilePrefab;
    public Transform overlayContainer;
    public Tilemap tilemap;

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
        }
    }

    void OnTileClicked(Vector3Int tilePos)
    {
        string overlayName = $"Overlay_{tilePos.x}_{tilePos.y}";
        Transform existingOverlay = overlayContainer.Find(overlayName);

        if (existingOverlay != null)
        {
            Destroy(existingOverlay.gameObject);
        }
        else
        {
            GameObject overlayTile = Instantiate(overlayTilePrefab, overlayContainer);
            overlayTile.name = overlayName;
            Vector3 cellWorldPosition = tilemap.GetCellCenterWorld(tilePos);
            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 0.1f);
        }
    }
}
