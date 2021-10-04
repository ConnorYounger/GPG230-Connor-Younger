using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W2Safe : MonoBehaviour
{
    public string safeCodeFirstHalf;
    public string safeCodeSecondHalf;

    public int[] codeNumbers;
    public int[] codeGeneratedNumbers;
    public TMP_Text[] codeTexts;

    // Start is called before the first frame update
    void Start()
    {
        codeGeneratedNumbers = new int[6];
        codeNumbers = new int[codeGeneratedNumbers.Length];
        GenerateCode();   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateCode()
    {
        for(int i = 0; i < codeGeneratedNumbers.Length; i++)
        {
            codeGeneratedNumbers[i] = Random.Range(0, 10);
        }

        safeCodeFirstHalf = codeGeneratedNumbers[0].ToString() + codeGeneratedNumbers[1].ToString() + codeGeneratedNumbers[2].ToString();
        safeCodeSecondHalf = codeGeneratedNumbers[3].ToString() + codeGeneratedNumbers[4].ToString() + codeGeneratedNumbers[5].ToString();
    }

    public void IncreaseValue(int i)
    {
        codeNumbers[i]++;

        if (codeNumbers[i] >= 10)
        {
            codeNumbers[i] = 0;
        }

        codeTexts[i].text = codeNumbers[i].ToString();
    }

    public void DecreaseValue(int i)
    {
        codeNumbers[i]--;

        if (codeNumbers[i] < 0)
        {
            codeNumbers[i] = 9;
        }

        codeTexts[i].text = codeNumbers[i].ToString();
    }

    bool EnterCorrectCode()
    {
        for(int i = 0; i < codeGeneratedNumbers.Length; i++)
        {
            if(codeNumbers[i] != codeGeneratedNumbers[i])
            {
                return false;
            }
        }

        return true;
    }

    public void EnterCode()
    {
        if (EnterCorrectCode())
        {
            Debug.Log("Correct code!");
        }
        else
        {
            Debug.Log("Incorrect code");
        }
    }
}
