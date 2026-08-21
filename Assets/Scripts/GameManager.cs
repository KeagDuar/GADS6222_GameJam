using NUnit.Framework.Internal;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] gameObjects;

    [Header("Prices")]
    private int waterBuildingCost = 200000;
    private int dataCenterBuildingCost = 150000;
    private int powerPlantCost = 120000;

    [Header("Counters")]
    private int waterBuildings;
    private int powerPlants;
    private int dataCenters;

    [Header("Resources")]
    private int water;
    private int electricity;
    private float money;
    public float Money => money;
    private float publicOpinion;
    private float environmentHealth;

    [Header("TickManagement")]
    float timer;
    float ticks = 1.0f;
    private void Awake()
    {
        EventManager.Water?.Invoke(water);
        EventManager.Electricity?.Invoke(electricity);
        EventManager.Money?.Invoke(money);
    }
    private void Start()
    {
        instance = this;
        waterBuildings = 0;
        powerPlants = 0;
        dataCenters = 0;
        electricity = 100000;
        water = 10000;
        money = 1000000;
        environmentHealth = 100;
        publicOpinion = 100;
    }
    private void OnEnable()
    {
        EventManager.BuildingTypeBought += HandleBuildingType;
    }
    private void OnDisable()
    {
        EventManager.BuildingTypeBought -= HandleBuildingType;
    }
    private void Update()
    {
        
            timer += Time.deltaTime;

            if (timer >= ticks)
            {
                GenerateResources();
                timer = 0.0f;
            }
    }

    private void HandleBuildingType(string buildingType)
    {
        switch (buildingType)
        {
            case "Water":
                money -= waterBuildingCost;
                waterBuildings++;
                break;
            case "Electricity":
                money -= powerPlantCost;
                powerPlants++; 
                break;
            case "Money":
                money -= dataCenterBuildingCost;
                dataCenters++; 
                break;
        }
    }

    private void GenerateResources()
    {
        if (waterBuildings > 0)
        {
            water += 100 * waterBuildings;
            electricity -= 50000 * waterBuildings;
        }

        if (powerPlants > 0)
        {
            electricity += 1400000 * powerPlants;
            water -= 200 * powerPlants;
        }

        if (dataCenters > 0)
        {
            electricity -= 100000 * dataCenters;
            water -= 15  * dataCenters;
            money += 0.6f * dataCenters;
            publicOpinion -= 0.5f * dataCenters;
            environmentHealth -= 0.5f * dataCenters;
        }
        EventManager.Water?.Invoke(water);
        EventManager.Electricity?.Invoke(electricity);
        EventManager.Money?.Invoke(money);
        EventManager.PublicOpinion?.Invoke(publicOpinion);
        EventManager.Environment?.Invoke(environmentHealth);

        if (publicOpinion <= 0 && environmentHealth <= 0)
        {
            Debug.Log("Is this the world you want to create?");

        }
    }

}
