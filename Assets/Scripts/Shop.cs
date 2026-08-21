using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    private int waterBuildingCost = 200000;
    private int dataCenterBuildingCost = 150000;
    private int powerPlantCost = 120000;
    private GameObject purchasingBuilding;

    public void Buy(GameObject building)
    {
        purchasingBuilding = building;

        float funds = GameManager.instance.Money;

        CheckFunds(funds);
    }

    private void CheckFunds(float funds)
    {
        if (purchasingBuilding == null)
            return;

        if (purchasingBuilding.gameObject.CompareTag("Water"))
        {
            if (funds >= waterBuildingCost)
            {
                EventManager.BuildingBought?.Invoke(purchasingBuilding);
                EventManager.NotEnoughMoney?.Invoke(false);
                purchasingBuilding = null;
            }
            else
            {
                EventManager.NotEnoughMoney?.Invoke(true);
            }
        }
        else if (purchasingBuilding.gameObject.CompareTag("Electricity"))
        {
            if (funds >= powerPlantCost)
            {
                EventManager.BuildingBought?.Invoke(purchasingBuilding);
                EventManager.NotEnoughMoney?.Invoke(false);
                purchasingBuilding = null;
            }
            else
            {
                EventManager.NotEnoughMoney?.Invoke(true);
            }
        }
        else if (purchasingBuilding.gameObject.CompareTag("Money"))
        {
            if (funds >= dataCenterBuildingCost)
            {
                EventManager.BuildingBought?.Invoke(purchasingBuilding);
                EventManager.NotEnoughMoney?.Invoke(false);
                purchasingBuilding = null;
            }
            else
            {
                EventManager.NotEnoughMoney?.Invoke(true);
            }
        }
    }
}
