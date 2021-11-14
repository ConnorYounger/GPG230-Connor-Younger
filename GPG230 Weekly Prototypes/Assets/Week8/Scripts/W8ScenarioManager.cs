using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W8ScenarioManager : MonoBehaviour
{
    public static BountyScenario currentScenario;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void StartNewScenario(BountyScenario scenario)
    {
        currentScenario = scenario;
        SceneManager.LoadScene(scenario.scenarioScene);
    }
}
