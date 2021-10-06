using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W2AchievementsManager : MonoBehaviour
{
    public Image[] achiveImages;

    public Color achivedColour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAchivementIcons()
    {
        if(PlayerPrefs.GetInt("keyWin") == 1)
        {
            achiveImages[0].color = achivedColour;
        }

        if (PlayerPrefs.GetInt("axeWin") == 1)
        {
            achiveImages[1].color = achivedColour;
        }

        if (PlayerPrefs.GetInt("ladderWin") == 1)
        {
            achiveImages[2].color = achivedColour;
        }

        if (PlayerPrefs.GetInt("allWin") == 1)
        {
            achiveImages[3].color = achivedColour;
        }

        if (PlayerPrefs.GetInt("secret1") == 1)
        {
            achiveImages[4].color = achivedColour;
        }
    }
}
