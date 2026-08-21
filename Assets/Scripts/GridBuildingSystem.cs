using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;
    public GridLayout gridLayout;
    public Tilemap mainTileMap;
    public Tilemap tempTilemap;

    public GameObject mainGrid;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;
    private bool placed;

    [Header("Counters")]
    public int moneyCount = 0;
    public int electCount = 0;
    public int waterCount = 0;

    private bool notEnoughMoney;

    private void OnEnable()
    {
        EventManager.BuildingBought += InitializeWithBuilding;
        EventManager.NotEnoughMoney += HandleNotEnoughMoney;
    }

    private void OnDisable()
    {
        EventManager.BuildingBought -= InitializeWithBuilding;
        EventManager.NotEnoughMoney -= HandleNotEnoughMoney;
    }
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        tileBases.Add(TileType.empty, null);
        tileBases.Add(TileType.white, Resources.Load<TileBase>("Tiles/white"));
        tileBases.Add(TileType.green, Resources.Load<TileBase>("Tiles/green"));
        tileBases.Add(TileType.red, Resources.Load<TileBase>("Tiles/red"));

        Debug.Log("White tile loaded: " + tileBases[TileType.white]);
        Debug.Log("Green tile loaded: " + tileBases[TileType.green]);
        Debug.Log("Red tile loaded: " + tileBases[TileType.red]);

        mainGrid.SetActive(false);
    }

    private void Update()
    {
        if (!temp)
            return;

        // Move building with mouse
        if (!temp.Placed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;

            Vector3Int cellPosition = gridLayout.WorldToCell(worldPosition);

            if (prevPos != cellPosition)
            {
                temp.transform.position =
                    gridLayout.CellToWorld(cellPosition);

                prevPos = cellPosition;

                FollowBuilding();
            }
        }

        // Place building
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("SPACE PRESSED");

            bool canPlace = temp.CanBePlaced();

            Debug.Log("Can place: " + canPlace);

            if (canPlace)
            {
                Debug.Log("PLACING BUILDING");
                temp.Place();
                placed = false;
                mainGrid.SetActive(false);
            }
        }

        // Cancel building
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClearArea();
            Destroy(temp.gameObject);
            temp = null;
            placed = false;
            mainGrid.SetActive(false);
        }
    }

    //TILE MAP MANAGEMENT

    private static TileBase[] GetTilesBlock(
        BoundsInt area,
        Tilemap tilemap)
    {
        TileBase[] array =
            new TileBase[area.size.x * area.size.y];

        int count = 0;

        foreach (Vector3Int position in area.allPositionsWithin)
        {
            array[count] = tilemap.GetTile(position);
            count++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tileMap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tileMap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    //BUILDING PLACEMENT

    public void InitializeWithBuilding(GameObject building)
    {

        if (!placed)
        {
            if (notEnoughMoney)
            {
                ClearArea();

                if (temp != null)
                {
                    Destroy(temp.gameObject);
                    temp = null;
                }

                placed = false;
                mainGrid.SetActive(false);
                Debug.Log("Not enough money");
                return;
            }

            mainGrid.SetActive(true);
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            if (building.gameObject.CompareTag("Money"))
            {
                moneyCount++;
            }
            else if (building.gameObject.CompareTag("Water"))
            {
                waterCount++;
            }
            else if (building.gameObject.CompareTag("Electricity"))
            {
                electCount++;
            }
            FollowBuilding();
            placed = true;
        }

    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.empty);
        tempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position =
            gridLayout.WorldToCell(temp.transform.position);

        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray =
            GetTilesBlock(buildingArea, mainTileMap);

        TileBase[] tileArray =
            new TileBase[baseArray.Length];

        bool canPlace = true;

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] != tileBases[TileType.white])
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            FillTiles(tileArray, TileType.green);
        }
        else
        {
            FillTiles(tileArray, TileType.red);
        }

        tempTilemap.SetTilesBlock(buildingArea, tileArray);

        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, mainTileMap);

        Debug.Log("Checking area: " + area);
        Debug.Log("Tiles found: " + baseArray.Length);

        foreach (var b in baseArray)
        {
            Debug.Log("Tile: " + b + " | Expected: " + tileBases[TileType.white]);

            if (b != tileBases[TileType.white])
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.empty, tempTilemap);
        SetTilesBlock(area, TileType.green, mainTileMap);      
    }

    private void HandleNotEnoughMoney(bool notEnough)
    {
        notEnoughMoney = notEnough;
    }
}

public enum TileType
{
    empty,
    white,
    green,
    red
}