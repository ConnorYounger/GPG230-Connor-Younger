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
    public GameObject shipUpgradeButtonPrefab;
    public Transform buttonGroup;
    public GameObject primaryWeaponsDevide;
    public GameObject secondaryWeaponsDevide;
    public TMP_Text shipNameText;
    public TMP_Text shipHullText;

    private List<GameObject> spawnedUpgradeUIElements;

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
        spawnedUpgradeUIElements = new List<GameObject>();
    }

    void Update()
    {
        
    }

    public void ShowShipSystems()
    {
        UpdateWeaponCurrency();

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
        ShipSaveData shipData = data.shipSaveData[data.currentShip];

        shipHullText.text = shipData.shipHull.ToString();

        // Spawn primary devide
        GameObject primaryDevide = Instantiate(primaryWeaponsDevide, buttonGroup.position, Quaternion.identity);
        primaryDevide.transform.parent = buttonGroup;
        spawnedUpgradeUIElements.Add(primaryDevide);

        // Spawn all primary weapon upgrade buttons
        foreach (ShipSaveData.weaponSlotData weaponSlot in shipData.primaryWeapons)
        {
            SpawnWeaponUpgradeUIButton(weaponSlot);
        }

        // Spawn secondary devide
        GameObject secondaryDevide = Instantiate(primaryWeaponsDevide, buttonGroup.position, Quaternion.identity);
        secondaryDevide.transform.parent = buttonGroup;
        spawnedUpgradeUIElements.Add(secondaryDevide);

        // Spawn all secondary weapon upgrade buttons
        foreach (ShipSaveData.weaponSlotData weaponSlot in shipData.secondaryWeapons)
        {
            SpawnWeaponUpgradeUIButton(weaponSlot);
        }
    }

    void SpawnWeaponUpgradeUIButton(ShipSaveData.weaponSlotData weaponSlot)
    {
        GameObject newSlot = Instantiate(shipUpgradeButtonPrefab, buttonGroup.position, Quaternion.identity);
        newSlot.transform.parent = buttonGroup;
        spawnedUpgradeUIElements.Add(newSlot);

        ShipUpgradeButton upgradeButton = newSlot.GetComponent<ShipUpgradeButton>();

        upgradeButton.shipLevelText.text = weaponSlot.weaponLevel.ToString();
        upgradeButton.shipNameText.text = weaponSlot.weaponName;
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
