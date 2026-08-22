using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityAction<GameObject> BuildingBought;
    public static UnityAction<string> BuildingTypeBought;
    public static UnityAction<bool> BuildingCancelled;
    public static UnityAction<bool> QTEGoodPressed;
    public static UnityAction<bool> QTEBadPressed;

    public static UnityAction<GameObject> ButtonClicked;

    //Resources
    public static UnityAction<int> Water;
    public static UnityAction<int> Electricity;
    public static UnityAction<float> Money;
    public static UnityAction<float> PublicOpinion;
    public static UnityAction<float> Environment;

    public static UnityAction<bool> NotEnoughMoney;

}
