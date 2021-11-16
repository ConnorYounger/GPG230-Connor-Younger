using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W8MainMenuManager : MonoBehaviour
{
    public GameObject currencyUI;
    public TMP_Text currencyText;

    [Header("Ship Systems")]
    public GameObject shipSystemsMenu;
    public GameObject shipUpgrades;
    public GameObject shipYard;

    [Header("Ship Upgrade Refs")]
    public TMP_Text shipNameText;
    public TMP_Text shipHullText;
    public ShipUpgradeButton primaryWeaponButton;
    public ShipUpgradeButton secondaryWeaponButton;

    private int[] assaltCannonUpgradeCosts = { 600, 1300, 2000};
    private int[] homingMissileCosts = { 600, 1300, 2000};

    private int primaryWeaponUpgradeCost;
    private int secondaryWeaponUpgradeCost;

    [Header("Contract Menus")]
    public GameObject contractsMenu;
    public GameObject contractInfo;

    [Header("Contract Info Refs")]
    public TMP_Text contractTitleText;
    public TMP_Text bountyValueText;
    public TMP_Text bountyDiscriptionText;
    public TMP_Text bountyDifficultyText;

    private BountyScenario currentScenario;
    public W8SaveData w8SaveData;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowShipSystems()
    {
        UpdateWeaponCurrency();
        ShowShipUpgrades();

        shipSystemsMenu.SetActive(true);
        currencyUI.SetActive(true);

        contractsMenu.SetActive(false);

        shipYard.SetActive(false);
        shipUpgrades.SetActive(true);
    }

    public void ShowContractsMenu()
    {
        UpdateWeaponCurrency();

        contractsMenu.SetActive(true);
        currencyUI.SetActive(true);

        shipSystemsMenu.SetActive(false);

        contractInfo.SetActive(false);
    }

    public void ShowContractInfoMenu()
    {
        contractInfo.SetActive(true);
    }

    public void ShowContractInfoMenu(BountyScenario contract)
    {
        currentScenario = contract;

        contractInfo.SetActive(true);

        contractTitleText.text = contract.bountyTitle;
        bountyValueText.text = contract.bountyValue.ToString();
        bountyDiscriptionText.text = contract.flavorText;
    }

    public void ShowShipUpgrades()
    {
        shipYard.SetActive(false);
        shipUpgrades.SetActive(true);

        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        //ShipSaveData shipData = data.shipSaveData[data.currentShip];

        shipHullText.text = data.shipHull[data.currentShip].ToString();

        primaryWeaponButton.shipLevelText.text = "Lv. " + (data.primaryWeaponLevel[data.currentShip] + 1).ToString();
        primaryWeaponButton.shipNameText.text = data.primaryWeaponName[data.currentShip];
        secondaryWeaponButton.shipLevelText.text = "Lv. " + (data.secondaryWeaponLevel[data.currentShip] + 1).ToString();
        secondaryWeaponButton.shipNameText.text = data.secondaryWeaponName[data.currentShip];

        UpdateWeaponButtonStats(data);
    }

    public void UpdateWeaponButtonStats(PlayerData data)
    {
        // Display weapon button stats

        Debug.Log(data.primaryWeaponName[data.currentShip]);

        if (data.primaryWeaponName[data.currentShip] == "AR-1")
        {
            primaryWeaponButton.upgradeCostText.text = assaltCannonUpgradeCosts[data.primaryWeaponLevel[data.currentShip]].ToString();
            primaryWeaponUpgradeCost = assaltCannonUpgradeCosts[data.primaryWeaponLevel[data.currentShip]];

            switch (data.primaryWeaponLevel[data.currentShip])
            {
                case 0:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "5 -> 10";
                    break;
                case 1:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "10 -> 15";
                    break;
                case 2:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "15 -> 20";
                    break;
                case 3:
                    primaryWeaponButton.button.interactable = false;
                    primaryWeaponButton.upgradeCostText.text = "--";
                    primaryWeaponButton.upgradeStatsText.text = "Max";
                    break;
            }
        }

        if (data.secondaryWeaponName[data.currentShip] == "HR-1")
        {
            primaryWeaponButton.upgradeCostText.text = homingMissileCosts[data.secondaryWeaponLevel[data.currentShip]].ToString();
            secondaryWeaponUpgradeCost = homingMissileCosts[data.secondaryWeaponLevel[data.currentShip]];

            switch (data.secondaryWeaponLevel[data.currentShip])
            {
                case 0:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "40 -> 70";
                    break;
                case 1:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "70 -> 100";
                    break;
                case 2:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "100 -> 130";
                    break;
                case 3:
                    primaryWeaponButton.button.interactable = false;
                    primaryWeaponButton.upgradeCostText.text = "--";
                    primaryWeaponButton.upgradeStatsText.text = "Max";
                    break;
            }
        }
    }

    public void PrimaryWeaponUpgradePress()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (data.w8PlayerCurrency >= primaryWeaponUpgradeCost)
        {
            W8SaveData.AddCurrency(-primaryWeaponUpgradeCost);

            w8SaveData.shipSaveData[data.currentShip].primaryWeapon.weaponLevel++;

            SaveSystem.SaveStats(w8SaveData);

            UpdateWeaponCurrency();
        }
    }

    public void ShowShipYard()
    {
        shipUpgrades.SetActive(false);
        shipYard.SetActive(true);
    }

    public void StartNewContract()
    {
        if (currentScenario)
        {
            W8ScenarioManager.StartNewScenario(currentScenario);
        }
    }

    public void UpdateWeaponCurrency()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (currencyText)
        {
            currencyText.text = data.w8PlayerCurrency.ToString();
        }
    }
}
