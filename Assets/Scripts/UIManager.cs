using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI _elect;
    public TextMeshProUGUI _water;
    public TextMeshProUGUI _money;
    public TextMeshProUGUI _environment;
    public TextMeshProUGUI _opinion;

    private int timer;
    private void Start()
    {
        HandleElect(100000);
        HandleWater(10000);
        HandleMoney(100000);
        HandleEnvironment(100);
        HandleOpinion(100);
    }
    private void OnEnable()
    {
        EventManager.Water += HandleWater;
        EventManager.Electricity += HandleElect;
        EventManager.Money += HandleMoney;
        EventManager.Environment += HandleEnvironment;
        EventManager.PublicOpinion += HandleOpinion;
    }

    private void OnDisable()
    {
        EventManager.Water -= HandleWater;
        EventManager.Electricity -= HandleElect;
        EventManager.Money -= HandleMoney;
        EventManager.Environment -= HandleEnvironment;
        EventManager.PublicOpinion -= HandleOpinion;
    }
    private void HandleElect(int electricity)
    {
        if (electricity <= 0)
        {
            _elect.color = Color.red;
        }
        _elect.text = electricity.ToString("N2", CultureInfo.InvariantCulture) + "W";
    }

    private void HandleWater(int water)
    {
        if (water <= 0)
        {
            _water.color = Color.red;
        }
        string waterDisplay = water.ToString("N2", CultureInfo.InvariantCulture);
        _water.text = waterDisplay;
    }

    private void HandleMoney(float money)
    {
        if (money <= 0)
        {
            _money.color = Color.red;
        }
        _money.text = "$" + money.ToString("N2", CultureInfo.InvariantCulture);
    }

    private void HandleEnvironment(float environement)
    {
        _environment.text = environement.ToString("N2", CultureInfo.InvariantCulture);
    }

    private void HandleOpinion(float opinion)
    {
        _opinion.text = opinion.ToString("N2", CultureInfo.InvariantCulture);
    }
}
