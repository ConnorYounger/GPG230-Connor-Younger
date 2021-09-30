using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W2Inventory : MonoBehaviour
{
    public W2Item key;
    public W2Item axe;
    public W2Item ladder;
    public W2Item map;
    public W2Item motionSensor;
    public W2Item sanityPills;

    public Image keyImg;
    public Image axeImg;
    public Image ladderImg;
    public Image mapImg;
    public Image motionSensorImg;
    public Image sanityPillsImg;

    public Color showColour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem(W2Item item)
    {
        switch (item.itemType.ToString())
        {
            case "key":
                if(key == null)
                    key = item;
                    keyImg.color = showColour;
                    AddItem(item);
                break;
            case "axe":
                if (axe == null)
                    axe = item;
                    axeImg.color = showColour;
                    AddItem(item);
                break;
            case "ladder":
                if (ladder == null)
                    ladder = item;
                    ladderImg.color = showColour;
                    AddItem(item);
                break;
            case "map":
                if (map == null)
                    map = item;
                    mapImg.color = showColour;
                    AddItem(item);
                break;
            case "motionSensor":
                if (motionSensor == null)
                    motionSensor = item;
                    motionSensorImg.color = showColour;
                    AddItem(item);
                break;
            case "santiyPills":
                if (sanityPills == null)
                    sanityPills = item;
                    sanityPillsImg.color = showColour;
                    AddItem(item);
                break;
        }

        Debug.Log("Added: " + item);
    }

    void AddItem(W2Item item)
    {
        item.gameObject.SetActive(false);
    }
}
