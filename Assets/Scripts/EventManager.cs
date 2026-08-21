using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static UnityAction<GameObject> BuildingBought;
    public static UnityAction<string> BuildingTypeBought;

    //Resources
    public static UnityAction<int> Water;
    public static UnityAction<int> Electricity;
    public static UnityAction<float> Money;
    public static UnityAction<float> PublicOpinion;
    public static UnityAction<float> Environment;

    public static UnityAction<bool> NotEnoughMoney;

}
