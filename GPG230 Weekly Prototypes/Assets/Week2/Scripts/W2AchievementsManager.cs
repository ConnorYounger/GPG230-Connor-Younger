using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class W2AchievementsManager : MonoBehaviour
{
    public Image[] achiveImages;

    public Color achivedColour;

    [Header("Achivement UI Refs")]
    public Image secret1IMG;
    public Sprite secret1Sprite;
    public TMP_Text s1AchivName;
    public TMP_Text s1AchivText;
    public Image secret2IMG;
    public TMP_Text s2AchivName;
    public TMP_Text s2AchivText;

    [Header("Achivement Pop-Ups")]
    public Animator popUpAnimator;
    public Sprite[] achivIcons;
    public Image spriteIcon;
    public TMP_Text title;
    public TMP_Text textField;

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
            secret1IMG.sprite = secret1Sprite;
            secret1IMG.color = Color.white;
            s1AchivName.text = "Excalibur";
            s1AchivText.text = "Traps the souls of it's victims";
        }

        if (PlayerPrefs.GetInt("secret2") == 1)
        {
            achiveImages[5].color = achivedColour;
            secret2IMG.color = Color.white;
            s2AchivName.text = "I'm So Cool";
            s2AchivText.text = "I'm so cool";
        }
    }

    public void UnlockAchievement(string achievement)
    {
        //Debug.LogError("Unlock Achiev: " + achievement);

        switch (achievement)
        {
            case "keyWin":
                if (PlayerPrefs.GetInt("keyWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[0]);
                PlayerPrefs.SetInt("keyWin", 1);
                break;
            case "axeWin":
                if (PlayerPrefs.GetInt("axeWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[1]);
                PlayerPrefs.SetInt("axeWin", 1);
                break;
            case "ladderWin":
                if (PlayerPrefs.GetInt("ladderWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[2]);
                PlayerPrefs.SetInt("ladderWin", 1);
                break;
            case "allWin":
                if (PlayerPrefs.GetInt("allWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[3]);
                PlayerPrefs.SetInt("allWin", 1);
                break;
            case "excalibur":
                if (PlayerPrefs.GetInt("excalibur") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[4]);
                PlayerPrefs.SetInt("secret1", 1);
                break;
            case "I'm So Cool":
                if (PlayerPrefs.GetInt("secret2") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[5]);
                PlayerPrefs.SetInt("secret2", 1);
                break;
        }
    }

    void DisplayAchievementUnlock(string achivName, Sprite icon)
    {
        //Debug.LogError("Display Achiev: " + achivName);

        title.text = achivName;
        textField.text = "Congratulations! You've unlocked the " + achivName + " achievement!";
        spriteIcon.sprite = icon;

        popUpAnimator.Play("AchiveMoveDown");
    }
}
