using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Week9/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;

    [System.Serializable]
    public struct questions
    {
        public string[] questionAnswers;
    }
    public questions[] question = new questions[4];

    public bool hasRoboticEyes;

    //public string[] conversation;
}
