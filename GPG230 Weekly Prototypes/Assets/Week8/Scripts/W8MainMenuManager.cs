using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W8MainMenuManager : MonoBehaviour
{
    public GameObject currencyUI;

    [Header("Ship Systems")]
    public GameObject shipSystemsMenu;
    public GameObject shipUpgrades;
    public GameObject shipYard;

    [Header("Contract Menus")]
    public GameObject contractsMenu;
    public GameObject contractInfo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowShipSystems()
    {
        shipSystemsMenu.SetActive(true);
        currencyUI.SetActive(true);

        contractsMenu.SetActive(false);

        shipYard.SetActive(false);
        shipUpgrades.SetActive(true);
    }

    public void ShowContractsMenu()
    {
        contractsMenu.SetActive(true);
        currencyUI.SetActive(true);

        shipSystemsMenu.SetActive(false);

        contractInfo.SetActive(false);
    }

    public void ShowContractInfoMenu()
    {
        contractInfo.SetActive(true);
    }

    public void ShowShipUpgrades()
    {
        shipYard.SetActive(false);
        shipUpgrades.SetActive(true);
    }

    public void ShowShipYard()
    {
        shipUpgrades.SetActive(false);
        shipYard.SetActive(true);
    }
}
