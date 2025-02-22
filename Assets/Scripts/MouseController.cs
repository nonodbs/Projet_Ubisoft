using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    // gestion des actions de la souris
    public Camera mainCamera;
    public Vector3 mapBoundsMin;
    public Vector3 mapBoundsMax;

    public GameObject cursorPrefab;
    public GameObject overlayTilePrefab;
    public Transform overlayContainer;
    public UIManager uimanager;
    public EvolutionManager evolutionManager;
    public Image INwait;

    public float ClickDuration = 2;
    bool clicking = false;
    float totalDownTime = 0;

    private GameObject cursorInstance;
    private List<Vector3Int> selectedTiles = new List<Vector3Int>();

    void Start()
    {
        if (cursorPrefab != null)
        {
            cursorInstance = Instantiate(cursorPrefab);
            cursorInstance.name = "CursorInstance";
        }
        mapBoundsMin = InfoManager.Instance.tilemap.localBounds.min;
        mapBoundsMax = InfoManager.Instance.tilemap.localBounds.max;
    }

    void Update()
    {
        if (uimanager.ispause)
        {
            return;
        }
        if (Cursor.visible)
        {
            //mets a jour la position du curseur en fonction de la position de la souris dans le jeu
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0.1f;

            Vector3Int tilePos = InfoManager.Instance.tilemap.WorldToCell(mouseWorldPosition);
            TileBase tile = InfoManager.Instance.tilemap.GetTile(tilePos);
            Vector3 newPos = InfoManager.Instance.tilemap.GetCellCenterWorld(tilePos);

            if (tile != null)
            {
                Vector3 offset = new Vector3(0, 0.1f, 0);
                newPos = newPos + offset;
                cursorInstance.transform.position = new Vector3(newPos.x, newPos.y, 1);
            }

            //action en fonction des clicks (souris pour selectionner, espace pour faire une action)
            if (Input.GetMouseButtonDown(0) && tile != null)
            {
                OnTileClicked(tilePos);
            }

            if (Input.GetMouseButtonDown(1) && tile != null)
            {
                mainCamera.transform.position = new Vector3(newPos.x, newPos.y, -10);
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
                    //click long: transforme les tiles en eau
                    ChangeSelectedTiles();
                    RemoveAllOverlays();
                    totalDownTime = 0;
                }
                else
                {
                    //click cour: évolué les tiles
                    EvolveSelectedTiles();
                    RemoveAllOverlays();
                    totalDownTime = 0;
                }
            }
            INwait.fillAmount = (totalDownTime / ClickDuration);     
        }
    }

    void OnTileClicked(Vector3Int tilePos)
    {
        //Selectionne les tiles
        string overlayName = $"Overlay_{tilePos.x}_{tilePos.y}";
        Transform existingOverlay = overlayContainer.Find(overlayName);

        if (existingOverlay == null)
        {
            if (overlayTilePrefab != null)
            {
                GameObject overlayTile = Instantiate(overlayTilePrefab, overlayContainer);
                overlayTile.name = overlayName;
                Vector3 cellWorldPosition = InfoManager.Instance.tilemap.GetCellCenterWorld(tilePos);
                Vector3 offset = new Vector3(0, 0.1f, 0);
                cellWorldPosition = cellWorldPosition + offset;
                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 1);
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
        //fait évolué les tiles
        foreach (var tilePos in selectedTiles)
        {
            evolutionManager.EvolveTile(tilePos);
        }

        selectedTiles.Clear();
    }

    void ChangeSelectedTiles()
    {
        //chage les tiles en eau
        foreach (var tilePos in selectedTiles)
        {
            evolutionManager.ChangeTileToWater(tilePos);
        }

        selectedTiles.Clear();
    }

    void RemoveAllOverlays()
    {
        //enlève les tiles selectionnées
        foreach (Transform child in overlayContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
