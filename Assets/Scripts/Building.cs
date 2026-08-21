using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType
    {
        Water,
        Electricity,
        Money //data center
    }

    public BuildingType type;

    public bool Placed { get; private set; }
    public BoundsInt area;

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
        switch (type)
        {
            case BuildingType.Water:
                EventManager.BuildingTypeBought?.Invoke("Water");
                Debug.Log("Water");
                break;
            case BuildingType.Electricity:
                EventManager.BuildingTypeBought?.Invoke("Electricity");
                Debug.Log("Elect");
                break;
            case BuildingType.Money:
                EventManager.BuildingTypeBought?.Invoke("Money");
                Debug.Log("Money");
                break;
        }
    }
}
