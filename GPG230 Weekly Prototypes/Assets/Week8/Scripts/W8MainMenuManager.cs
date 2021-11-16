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

    [Header("Contract Menus")]
    public GameObject contractsMenu;
    public GameObject contractInfo;

    [Header("Contract Info Refs")]
    public TMP_Text contractTitleText;
    public TMP_Text bountyValueText;
    public TMP_Text bountyDiscriptionText;
    public TMP_Text bountyDifficultyText;

    private BountyScenario currentScenario;

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
            switch (data.primaryWeaponLevel[data.currentShip])
            {
                case 0:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeCostText.text = assaltCannonUpgradeCosts[0].ToString();
                    primaryWeaponButton.upgradeStatsText.text = "5 -> 10";
                    break;
                case 1:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "10 -> 15";
                    primaryWeaponButton.upgradeCostText.text = assaltCannonUpgradeCosts[1].ToString();
                    break;
                case 2:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "15 -> 20";
                    primaryWeaponButton.upgradeCostText.text = assaltCannonUpgradeCosts[2].ToString();
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
            switch (data.secondaryWeaponLevel[data.currentShip])
            {
                case 0:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeCostText.text = homingMissileCosts[0].ToString();
                    primaryWeaponButton.upgradeStatsText.text = "40 -> 70";
                    break;
                case 1:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "70 -> 100";
                    primaryWeaponButton.upgradeCostText.text = homingMissileCosts[1].ToString();
                    break;
                case 2:
                    primaryWeaponButton.button.interactable = true;
                    primaryWeaponButton.upgradeStatsText.text = "100 -> 130";
                    primaryWeaponButton.upgradeCostText.text = homingMissileCosts[2].ToString();
                    break;
                case 3:
                    primaryWeaponButton.button.interactable = false;
                    primaryWeaponButton.upgradeCostText.text = "--";
                    primaryWeaponButton.upgradeStatsText.text = "Max";
                    break;
            }
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
