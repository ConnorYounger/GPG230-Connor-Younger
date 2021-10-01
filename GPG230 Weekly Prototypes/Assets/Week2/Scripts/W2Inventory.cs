using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W2Inventory : MonoBehaviour
{
    public List<W2Interractable> items;

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
        items = new List<W2Interractable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem(W2Interractable item)
    {
        if (item.isItem)
        {
            switch (item.interractableType.ToString())
            {
                case "key":
                    keyImg.color = showColour;
                    break;
                case "axe":
                    axeImg.color = showColour;
                    break;
                case "ladder":
                    ladderImg.color = showColour;
                    break;
                case "map":
                    mapImg.color = showColour;
                    break;
                case "motionSensor":
                    motionSensorImg.color = showColour;
                    break;
                case "santiyPills":
                    sanityPillsImg.color = showColour;
                    break;
            }

            AddItem(item);
        }

        Debug.Log("Added: " + item);
    }

    void AddItem(W2Interractable item)
    {
        item.gameObject.SetActive(false);
        items.Add(item);
    }

    public W2Interractable SearchForItem(string itemName)
    {
        W2Interractable foundItem = null;

        foreach (W2Interractable item in items)
        {
            if (item.interractableType.ToString() == itemName)
                foundItem = item;
        }

        return foundItem;
    }
}
