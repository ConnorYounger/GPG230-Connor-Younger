using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class W8MainMenuManager : MonoBehaviour
{
    public GameObject currencyUI;
    public TMP_Text currencyText;
    public MultiplayerConnectManager multiplayerConnectManager;

    [Header("Ship Systems")]
    public GameObject shipSystemsMenu;
    public GameObject shipUpgrades;
    public GameObject shipYard;
    public GameObject multiplayerUI;
    public GameObject[] smallButtons;

    [Header("Ship Upgrade Refs")]
    public TMP_Text shipNameText;
    public TMP_Text shipHullText;
    public ShipUpgradeButton primaryWeaponButton;
    public ShipUpgradeButton secondaryWeaponButton;

    private int[] assaltCannonUpgradeCosts = { 600, 1300, 2000};
    private int[] homingMissileCosts = { 600, 1300, 2000};

    private int primaryWeaponUpgradeCost;
    private int secondaryWeaponUpgradeCost;

    [Header("Ship Yard Refs")]
    public bool[] shipYardButtons;
    public GameObject shipBuyMenu;
    public TMP_Text shipTitle;
    public TMP_Text shipDiscription;
    public TMP_Text shipBuyButtonText;
    public TMP_Text shipCostText;

    public Image[] shipSprites;

    [TextArea]
    public string[] shipDiscriptions;

    private int[] shipCosts = { 0, 3000, 6000, 12000 };

    private int shipUICurrentIndex;

    [Header("Contract Menus")]
    public GameObject contractsMenu;
    public GameObject contractInfo;

    [Header("Contract Info Refs")]
    public TMP_Text contractTitleText;
    public TMP_Text bountyValueText;
    public TMP_Text bountyDiscriptionText;
    public TMP_Text bountyDifficultyText;

    [Header("Help Menu")]
    public GameObject helpMenu;

    [Header("Ship Models")]
    public GameObject[] shipModels;

    private BountyScenario currentScenario;
    public W8SaveData w8SaveData;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] buttonPressSound;
    public AudioClip buttonFailSound;

    void Start()
    {
        //w8SaveData.shipsUnlocked[2] = true;
        //SaveSystem.SaveStats(w8SaveData);

        //PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        //foreach(bool b in data.shipsUnlocked)
        //{
        //    Debug.Log(b);
        //}

        UpdateShipYardButtons();

        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        SwitchShips(data.currentShip);

        UpdateShipSprites();
    }

    void Update()
    {
        
    }

    void UpdateShipYardButtons()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if(shipYardButtons.Length > 0)
        {
            for(int i = 0; i < data.shipsUnlocked.Length; i++)
            {
                shipYardButtons[i] = data.shipsUnlocked[i];
            }
        }
    }

    public void ShowShipSystems()
    {
        UpdateWeaponCurrency();
        //ShowShipUpgrades();

        shipSystemsMenu.SetActive(true);
        currencyUI.SetActive(true);

        contractsMenu.SetActive(false);
        helpMenu.SetActive(false);

        shipYard.SetActive(false);
        shipBuyMenu.SetActive(false);
        shipUpgrades.SetActive(true);
        multiplayerUI.SetActive(false);

        ShowShipYard();
        UpdateSmallButtonsUI(0);

        PlayButtonPressedSound();
    }

    public void ShowContractsMenu()
    {
        UpdateSmallButtonsUI(0);

        UpdateWeaponCurrency();

        contractsMenu.SetActive(true);
        currencyUI.SetActive(true);

        shipSystemsMenu.SetActive(false);
        helpMenu.SetActive(false);
        multiplayerUI.SetActive(false);

        contractInfo.SetActive(false);

        PlayButtonPressedSound();
    }

    public void ShowHelpMenu()
    {
        UpdateWeaponCurrency();

        shipSystemsMenu.SetActive(false);
        contractsMenu.SetActive(false);
        shipYard.SetActive(false);
        shipUpgrades.SetActive(false);
        multiplayerUI.SetActive(false);

        helpMenu.SetActive(true);

        PlayButtonPressedSound();
    }

    public void ShowMultiplayerMenu()
    {
        UpdateWeaponCurrency();
        //ShowShipUpgrades();

        multiplayerUI.SetActive(true);
        shipSystemsMenu.SetActive(false);
        currencyUI.SetActive(true);

        contractsMenu.SetActive(false);
        helpMenu.SetActive(false);

        shipYard.SetActive(false);
        shipBuyMenu.SetActive(false);

        UpdateSmallButtonsUI(0);

        PlayButtonPressedSound();
    }

    public void ShowContractInfoMenu()
    {
        contractInfo.SetActive(true);

        PlayButtonPressedSound();
    }

    public void ShowContractInfoMenu(BountyScenario contract)
    {
        UpdateSmallButtonsUI(1);

        currentScenario = contract;

        contractInfo.SetActive(true);

        contractTitleText.text = contract.bountyTitle;
        bountyValueText.text = contract.bountyValue.ToString();
        bountyDiscriptionText.text = contract.flavorText;

        PlayButtonPressedSound();
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

        //Debug.Log(data.primaryWeaponName[data.currentShip]);

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

    void UpdateSmallButtonsUI(int i)
    {
        if(i == 0)
        {
            smallButtons[0].SetActive(true);
            smallButtons[1].SetActive(false);
        }
        else
        {
            smallButtons[0].SetActive(false);
            smallButtons[1].SetActive(true);
        }
    }

    public void PrimaryWeaponUpgradePress()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (data.w8PlayerCurrency >= primaryWeaponUpgradeCost)
        {
            W8SaveData.AddCurrency(-primaryWeaponUpgradeCost);

            w8SaveData.LoadSave();
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
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
                multiplayerConnectManager.hasConnected = false;
            }

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

        Debug.Log(data.currentShip);
    }

    public void SwitchShips(int shipIndex)
    {
        if (w8SaveData)
        {
            w8SaveData.LoadSave();
            w8SaveData.currentShip = shipIndex;
            SaveSystem.SaveStats(w8SaveData);
        }

        //Debug.Log("Switch to ship index: " + shipIndex);

        for(int i = 0; i < shipModels.Length; i++)
        {
            if(i == shipIndex)
            {
                shipModels[i].SetActive(true);
            }
            else
            {
                shipModels[i].SetActive(false);
            }
        }
    }

    public void ShowShipUI(int shipIndex)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        shipUICurrentIndex = shipIndex;
        shipDiscription.text = shipDiscriptions[shipIndex];

        switch (shipIndex)
        {
            case 0:
                shipTitle.text = "Kestrel";
                break;
            case 1:
                shipTitle.text = "Nostromo";
                break;
            case 2:
                shipTitle.text = "Avant Heim";
                break;
            case 3:
                shipTitle.text = "Sword Fish";
                break;
        }

        if (data.shipsUnlocked[shipIndex])
        {
            shipBuyButtonText.text = "Equip";
            shipCostText.text = "Unlocked";
        }
        else
        {
            shipBuyButtonText.text = "Buy";
            shipCostText.text = shipCosts[shipIndex].ToString();
        }

        shipBuyMenu.SetActive(true);

        PlayButtonPressedSound();
    }

    public void ShipButtonInteract()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (data.shipsUnlocked[shipUICurrentIndex])
        {
            SwitchShips(shipUICurrentIndex);

            PlayButtonPressedSound();
        }
        else
        {
            ShipCost(shipCosts[shipUICurrentIndex]);
        }
    }

    void ShipCost(int cost)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (data.w8PlayerCurrency >= cost)
        {
            W8SaveData.AddCurrency(-cost);

            w8SaveData.LoadSave();
            w8SaveData.shipsUnlocked[shipUICurrentIndex] = true;
            SaveSystem.SaveStats(w8SaveData);

            SwitchShips(shipUICurrentIndex);
            UpdateWeaponCurrency();
            shipBuyButtonText.text = "Equip";
            shipCostText.text = "Unlocked";

            PlayButtonPressedSound();

            UpdateShipSprites();
        }
        else
        {
            PlayButtonFailSound();
        }
    }

    public void PlayButtonPressedSound()
    {
        int rand = Random.Range(0, buttonPressSound.Length);
        audioSource.clip = buttonPressSound[rand];
        audioSource.Play();
    }

    public void PlayButtonFailSound()
    {
        audioSource.clip = buttonFailSound;
        audioSource.Play();
    }

    void UpdateShipSprites()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        for (int i = 0; i < data.shipsUnlocked.Length; i++)
        {
            if (data.shipsUnlocked[i])
            {
                shipSprites[i].color = Color.white;
            }
            else
            {
                shipSprites[i].color = Color.black;
            }
        }
    }
}
