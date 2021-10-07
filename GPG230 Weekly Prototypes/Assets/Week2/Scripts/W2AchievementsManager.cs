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
            s1AchivName.text = "Excalibur";
            s1AchivText.text = "Traps the souls of it's victoms";
        }
    }

    public void UnlockAchievement(string achievement)
    {
        switch (achievement)
        {
            case "keyWn":
                if(PlayerPrefs.GetInt("keyWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[0]);
                break;
            case "axeWn":
                if (PlayerPrefs.GetInt("axeWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[1]);
                break;
            case "ladderWn":
                if (PlayerPrefs.GetInt("ladderWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[2]);
                break;
            case "allWn":
                if (PlayerPrefs.GetInt("allWin") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[3]);
                break;
            case "excalibur":
                if (PlayerPrefs.GetInt("excalibur") == 0)
                    DisplayAchievementUnlock(achievement, achivIcons[4]);
                break;
        }
    }

    void DisplayAchievementUnlock(string achivName, Sprite icon)
    {
        title.text = achivName;
        textField.text = "Congratulations! You've unlocked the " + achivName + " achievement!";
        spriteIcon.sprite = icon;

        popUpAnimator.Play("AchiveMoveDown");
    }
}
