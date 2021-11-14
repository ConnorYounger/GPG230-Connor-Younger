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

    public int returnTime = 5;
    private int returnTimer;

    void Start()
    {
        returnTimer = returnTime;
    }

    void Update()
    {
        
    }

    public static void StartNewScenario(BountyScenario scenario)
    {
        currentScenario = scenario;
        SceneManager.LoadScene(scenario.scenarioScene);
    }

    public void ShowWinUI()
    {
        rewardText.text = currentScenario.bountyValue.ToString();
        WinUI.SetActive(true);
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
