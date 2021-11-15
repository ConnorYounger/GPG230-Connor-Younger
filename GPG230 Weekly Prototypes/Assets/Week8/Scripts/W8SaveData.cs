using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W8SaveData : MonoBehaviour
{
    public static int playerScore;
    public static string savePath = "/W8SaveData";

    // Start is called before the first frame update
    void Start()
    {
        //SaveSystem.SaveStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddCurrency(100);
        }
    }

    public static void AddCurrency(int amount)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        playerScore = data.w8PlayerCurrency;

        // Save level stats
        if (data != null)
        {
            playerScore += amount;
            //Debug.Log("Add: " + amount.ToString() + ", Total: " + data.w8PlayerCurrency);

            SaveSystem.SaveStats();
        }
        else
        {
            Debug.LogError("Could not load " + data);
        }
    }

    public void LoadSave()
    {
        //PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        //if (currencyText)
        //{
        //    currencyText.text = data.w8PlayerCurrency.ToString();
        //}
    }
}
