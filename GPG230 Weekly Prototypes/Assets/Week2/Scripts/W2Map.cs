using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W2Map : MonoBehaviour
{
    public Image[] playerLocations;

    public int roomIndex;

    private void Start()
    {
        SetRoom(12);
    }

    public void SetRoom(int index)
    {
        roomIndex = index;

        DisplayPlayerLocation();
    }

    void DisplayPlayerLocation()
    {
        for(int i = 0; i < playerLocations.Length; i++)
        {
            if(i == roomIndex)
            {
                playerLocations[i].enabled = true;
            }
            else
            {
                playerLocations[i].enabled = false;
            }
        }
    }
}
