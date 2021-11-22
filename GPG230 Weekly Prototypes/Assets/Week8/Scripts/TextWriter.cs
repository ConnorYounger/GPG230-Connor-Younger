using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    private TMP_Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;

    public void AddWritter(TMP_Text text, string textTW, float timePC, bool invisChar)
    {
        uiText = text;
        textToWrite = textTW;
        timePerCharacter = timePC;
        invisibleCharacters = invisChar;
        characterIndex = 0;
    }

    private void Update()
    {
        TextTimer();
    }

    void TextTimer()
    {
        if(uiText != null)
        {
            timer -= Time.deltaTime;

            while(timer <= 0)
            {
                timer += timePerCharacter;
                characterIndex++;
                string text = uiText.text = textToWrite.Substring(0, characterIndex);

                if (invisibleCharacters)
                {
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                uiText.text = text;

                if(characterIndex >= textToWrite.Length)
                {
                    uiText = null;
                    return;
                }
            }
        }
    }
}
