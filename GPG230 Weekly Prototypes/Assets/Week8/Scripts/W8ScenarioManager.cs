using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class W8ScenarioManager : MonoBehaviour
{
    public static BountyScenario currentScenario;

    public GameObject WinUI;
    public TMP_Text rewardText;
    public TMP_Text returnText;

    public EnemyShipSpawnManager[] enemySpawnManagers;

    public int returnTime = 5;
    private int returnTimer;

    void Start()
    {
        returnTimer = returnTime;

        SpawnEnemies();
    }

    void Update()
    {
        
    }

    public static void StartNewScenario(BountyScenario scenario)
    {
        currentScenario = scenario;
        SceneManager.LoadScene(scenario.scenarioScene);
    }

    public void SpawnEnemies()
    {
        switch (currentScenario.name)
        {
            case "Scenario1":
                enemySpawnManagers[0].SpawnEnemies();
                break;
            case "Scenario2":
                enemySpawnManagers[0].SpawnEnemies();
                break;
            case "Scenario3":
                enemySpawnManagers[0].SpawnEnemies();
                break;
            case "Scenario4":
                enemySpawnManagers[1].SpawnEnemies();
                break;
            case "Scenario5":
                enemySpawnManagers[1].SpawnEnemies();
                break;
            case "Scenario6":
                enemySpawnManagers[0].SpawnEnemies();
                break;
            default:
                enemySpawnManagers[0].SpawnEnemies();
                break;
        }
    }

    public void ShowWinUI()
    {
        rewardText.text = currentScenario.bountyValue.ToString();
        W8SaveData.AddCurrency(currentScenario.bountyValue);
        WinUI.SetActive(true);
        StopCoroutine("ReturnCounter");
        StartCoroutine("ReturnCounter");
    }

    IEnumerator ReturnCounter()
    {
        returnText.text = "Returning in: " + returnTimer.ToString() + "s";

        yield return new WaitForSeconds(1);

        returnTimer--;

        if(returnTimer >= 0)
        {
            StartCoroutine("ReturnCounter");
        }
        else
        {
            currentScenario = null;
            SceneManager.LoadScene("Week8MainMenu");
        }
    }
}
