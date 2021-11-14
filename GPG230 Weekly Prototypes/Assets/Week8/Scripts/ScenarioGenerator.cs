using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioGenerator : MonoBehaviour
{
    public BountyScenario[] scenarios;

    public BountyScenario generatedScenario;

    [Header("Refs")]
    public TMP_Text scenarioTitle;
    public TMP_Text bountyReward;

    public W8MainMenuManager menuManager;

    void Start()
    {
        GenerateRandomScenario();
    }

    void GenerateRandomScenario()
    {
        int rand = Random.Range(0, scenarios.Length);
        generatedScenario = scenarios[rand];
        SetUIRefs();
    }

    void SetUIRefs()
    {
        scenarioTitle.text = generatedScenario.bountyTitle;
        bountyReward.text = generatedScenario.bountyValue.ToString();
    }

    public void SelectContract()
    {
        menuManager.ShowContractInfoMenu(generatedScenario);
    }
}
