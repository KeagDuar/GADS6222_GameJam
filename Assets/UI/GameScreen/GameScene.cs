using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;
using static UnityEngine.Rendering.DebugUI.MessageBox;
public class MaiGameScene : MonoBehaviour   //Main menu scene
{
    public UIDocument document;
    private Button electButton;
    private Button waterButton;
    private Button dataCenterButton;
    public GameObject powerPlant;
    public GameObject waterTreatment;
    public GameObject dataCenter;

    private Label elecLbl;
    private Label waterlbl;
    private Label dataCenterlbl;
    private Label environmentlbl;
    private Label opinionlbl;

    public VisualElement buildingInfoPanel;
    private Label buildingInfoLabel;
    private bool moveInfoPanel;
    private bool cancelInfoPanel;

    [Header("QTE's")]
    public VisualElement qtePanel;
    private Button GoodButton;
    private Button BadButton;
    private Label QTEText;
    private Image QTEImage;
    private int dataCenterCount;


    private void Awake()
    {
        VisualElement gameSceneRoot = document.rootVisualElement;

        electButton = gameSceneRoot.Q<Button>("Electricity");
        waterButton = gameSceneRoot.Q<Button>("Water");
        dataCenterButton = gameSceneRoot.Q<Button>("DataCenter");

        elecLbl = gameSceneRoot.Q<Label>("lblElectricity");
        waterlbl = gameSceneRoot.Q<Label>("lblWater");
        dataCenterlbl = gameSceneRoot.Q<Label>("lblDataCenter");
        environmentlbl = gameSceneRoot.Q<Label>("lblEnvHealth");
        opinionlbl = gameSceneRoot.Q<Label>("lblOpinion");

        buildingInfoPanel = gameSceneRoot.Q<VisualElement>("BuildingInfoPanel");
        buildingInfoLabel = gameSceneRoot.Q<Label>("BuildingInfoLabel");
        buildingInfoPanel.style.display = DisplayStyle.None;

        dataCenterlbl.style.color = Color.yellow;
        dataCenterlbl.style.color = Color.blue;
        dataCenterlbl.style.color = Color.green;
        environmentlbl.style.color = Color.brown;
        opinionlbl.style.color = Color.mediumPurple;

        //QTE's
        qtePanel = gameSceneRoot.Q<VisualElement>("QTEPanel");
        qtePanel.style.display = DisplayStyle.None;
        GoodButton = gameSceneRoot.Q<Button>("GoodButton");
        BadButton = gameSceneRoot.Q<Button>("BadButton");
        QTEText = gameSceneRoot.Q<Label>("QTEText");
        QTEImage = gameSceneRoot.Q<Image>("QTEImage");
        GoodButton.clicked += OnGoodButtonClicked;
        BadButton.clicked += OnBadButtonClicked;

        electButton.clicked += OnElectricityClicked;
        waterButton.clicked += OnWaterClicked;
        dataCenterButton.clicked += OnDataCenterClicked;
        moveInfoPanel = true;
    }
    private void Start()
    {
        cancelInfoPanel = false;
        dataCenterCount = 0;
        HandleElect(100000);
        HandleWater(10000);
        HandleMoney(100000);
        HandleEnvironment(100);
        HandleOpinion(100);
    }
   

    private void Update()
    {
        if (buildingInfoPanel.style.display == DisplayStyle.Flex && moveInfoPanel)
        {
            UpdateBuildingInfoPosition();
        }

    }
    private void OnElectricityClicked()
    {
        moveInfoPanel = true;
        buildingInfoPanel.style.display = DisplayStyle.Flex;

        buildingInfoLabel.text =
            $"Cost: $120,000\n" +
            $"Produces: 1,400,000W\n" +
            $"Consumes: 200L";

        EventManager.ButtonClicked?.Invoke(powerPlant);
    }
    private void OnWaterClicked()
    {
        moveInfoPanel = true;

        buildingInfoLabel.text =
            $"Cost: $200,000\n" +
            $"Produces: 100L\n" +
            $"Consumes: 50,000W";

        buildingInfoPanel.style.display = DisplayStyle.Flex;


        EventManager.ButtonClicked?.Invoke(waterTreatment);
    }
    private void OnDataCenterClicked()
    {
        dataCenterCount++;

        if (dataCenterCount == 3)
        {
            GameManager.instance.paused = true;
            qtePanel.style.display = DisplayStyle.Flex;
        }
        moveInfoPanel = true;
        buildingInfoPanel.style.display = DisplayStyle.Flex;

        buildingInfoLabel.text =
            "Cost: $150,000\n" +
            "Produces: $0.6\n" +
            "Consumes: 100,000W\n" +
            "Consumes: 15L\n" +
            "Public opinion: -0.5\n" +
            "Environment health: -0.5";

        EventManager.ButtonClicked?.Invoke(dataCenter);
    }

    private void OnGoodButtonClicked()
    {
        qtePanel.style.display = DisplayStyle.None;
        buildingInfoPanel.style.display = DisplayStyle.None;
        EventManager.QTEGoodPressed?.Invoke(true);
        EventManager.QTEBadPressed?.Invoke(false);
        GameManager.instance.paused = false;
    }
    private void OnBadButtonClicked()
    {
        qtePanel.style.display = DisplayStyle.None;
        buildingInfoPanel.style.display = DisplayStyle.None;
        EventManager.QTEGoodPressed?.Invoke(false);
        EventManager.QTEBadPressed?.Invoke(true);
        GameManager.instance.paused = false;
    }

    private void OnEnable()
    {
        EventManager.Water += HandleWater;
        EventManager.Electricity += HandleElect;
        EventManager.Money += HandleMoney;
        EventManager.Environment += HandleEnvironment;
        EventManager.PublicOpinion += HandleOpinion;

        EventManager.BuildingTypeBought += BuildingBought;
        EventManager.BuildingCancelled += HandleCancel;

        EventManager.NotEnoughMoney += HandleNotEnoughMoney;
    }

    private void OnDisable()
    {
        EventManager.Water -= HandleWater;
        EventManager.Electricity -= HandleElect;
        EventManager.Money -= HandleMoney;
        EventManager.Environment -= HandleEnvironment;
        EventManager.PublicOpinion -= HandleOpinion;

        EventManager.BuildingTypeBought -= BuildingBought;
        EventManager.BuildingCancelled -= HandleCancel;

        EventManager.NotEnoughMoney -= HandleNotEnoughMoney;
    }
    private void HandleElect(int electricity)
    {
        if (electricity <= 0)
        {
            elecLbl.style.color = Color.red;
        }
        else
            elecLbl.style.color = Color.yellow;

        elecLbl.text = electricity.ToString("N2", CultureInfo.InvariantCulture) + "W";
    }

    private void HandleWater(int water)
    {
        if (water <= 0)
        {
            waterlbl.style.color = Color.red;
        }
        else
            waterlbl.style.color = Color.blue;

        waterlbl.text = water.ToString("N2", CultureInfo.InvariantCulture) + "L";
    }

    private void HandleMoney(float money)
    {
        if (money <= 0)
        {
            dataCenterlbl.style.color = Color.red;
        }
        else
            dataCenterlbl.style.color = Color.green;

        dataCenterlbl.text = "$" + money.ToString("N2", CultureInfo.InvariantCulture);
    }

    private void HandleEnvironment(float environement)
    {
        if (environement <= 0)
        {
            environmentlbl.style.color = Color.red;
        }
        else
            environmentlbl.style.color = Color.brown;

        environmentlbl.text = environement.ToString("N2", CultureInfo.InvariantCulture);
    }

    private void HandleOpinion(float opinion)
    {
        if (opinion <= 0)
        {
            opinionlbl.style.color = Color.red;
        }
        else
            opinionlbl.style.color = Color.mediumPurple;

        opinionlbl.text = opinion.ToString("N2", CultureInfo.InvariantCulture);
    }

    private void UpdateBuildingInfoPosition()
    {
        if (buildingInfoPanel == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float flippedY = Screen.height - mousePosition.y;

        Vector2 panelPosition =
            RuntimePanelUtils.ScreenToPanel(
                document.rootVisualElement.panel,
                new Vector2(mousePosition.x, flippedY)
            );

        buildingInfoPanel.style.left = panelPosition.x + 20;
        buildingInfoPanel.style.top = panelPosition.y + 20;
    }

    private void BuildingBought(string objectBought)
    {
        buildingInfoPanel.style.display = DisplayStyle.None;
        moveInfoPanel = false;
    }

    private void HandleCancel(bool cancelBuild)
    {
        if (!cancelBuild) 
            return;

        buildingInfoPanel.style.display = DisplayStyle.None;
        moveInfoPanel = false;
    }

    private void HandleNotEnoughMoney(bool notEnough)
    {
        Debug.Log("UI RECEIVED NOT ENOUGH MONEY: " + notEnough);

        if (!notEnough)
            return;

        buildingInfoPanel.style.display = DisplayStyle.None;
        moveInfoPanel = false;
    }
}
