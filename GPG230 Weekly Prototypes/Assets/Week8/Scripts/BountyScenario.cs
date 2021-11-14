using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scenario", menuName = "Week8/BountyScenario", order = 1)]
public class BountyScenario : ScriptableObject
{
    public string bountyTitle;

    public int bountyValue;

    public string flavorText;

    public string scenarioScene;
}
