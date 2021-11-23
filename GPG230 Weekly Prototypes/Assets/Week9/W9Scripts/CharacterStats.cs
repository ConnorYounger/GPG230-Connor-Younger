using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Week9/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;

    public Sprite characterSprite;

    [System.Serializable]
    public struct questions
    {
        public string[] questionAnswers;
    }
    public questions[] question = new questions[4];

    public bool hasRoboticEyes;

    //public string[] conversation;
}
