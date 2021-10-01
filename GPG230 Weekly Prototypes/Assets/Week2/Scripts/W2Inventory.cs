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

    public Image axeBodyImage;
    public Image axeHeadImage;
    public Image axeDuckTape;
    public Image ladderDuckTape;
    public Image ladderHalf1;
    public Image ladderHalf2;

    public Button craftableAxe;
    public Button craftableLadder;

    public Color showColour;
    public Color hideColour;

    private int duckTapeCount;

    private bool axeBodyCollected;
    private bool axeHeadCollected;
    private bool ladder1Collected;
    private bool ladder2Collected;
    private bool axeCrafted;
    private bool ladderCrafted;

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
                case "duckTape":
                    duckTapeCount++;
                    UpdateDuckTapeUI();
                    CheckAxeCraftable();
                    CheckLadderCraftable();
                    break;
                case "axeBody":
                    if (!axeCrafted)
                    {
                        axeBodyImage.color = showColour;
                        axeBodyCollected = true;
                        CheckAxeCraftable();
                    }
                    break;
                case "axeHead":
                    if (!ladderCrafted)
                    {
                        axeHeadImage.color = showColour;
                        axeHeadCollected = true;
                        CheckAxeCraftable();
                    }
                    break;
                case "ladderBottom":
                    if (!ladderCrafted)
                    {
                        ladderHalf1.color = showColour;
                        ladder1Collected = true;
                        CheckLadderCraftable();
                    }
                    break;
                case "ladderTop":
                    if (!ladderCrafted)
                    {
                        ladderHalf2.color = showColour;
                        ladder2Collected = true;
                        CheckLadderCraftable();
                    }
                    break;
            }

            AddItem(item);
        }

        Debug.Log("Added: " + item);
    }

    public void CraftAxe()
    {
        // Item creation
        GameObject newItem = Instantiate(new GameObject(), transform.position, transform.rotation);
        newItem.AddComponent<Outline>();
        newItem.name = "AxeItem";
        W2Interractable axe = newItem.AddComponent<W2Interractable>();
        axe.isItem = true;
        axe.CreateItem("axe");
        PickUpItem(axe);
        axeCrafted = true;
        craftableAxe.interactable = false;

        // Material costs
        axeBodyCollected = false;
        axeHeadCollected = false;
        axeHeadImage.color = hideColour;
        axeBodyImage.color = hideColour;
        duckTapeCount--;
        UpdateDuckTapeUI();
    }

    public void CraftLadder()
    {
        // Item creation
        GameObject newItem = Instantiate(new GameObject(), transform.position, transform.rotation);
        newItem.AddComponent<Outline>();
        newItem.name = "LadderItem";
        W2Interractable ladder = newItem.AddComponent<W2Interractable>();
        ladder.isItem = true;
        ladder.CreateItem("ladder");
        PickUpItem(ladder);
        ladderCrafted = true;
        craftableLadder.interactable = false;

        // Material costs
        ladder1Collected = false;
        ladder2Collected = false;
        ladderHalf1.color = hideColour;
        ladderHalf2.color = hideColour;
        duckTapeCount--;
        UpdateDuckTapeUI();
    }

    void CheckAxeCraftable()
    {
        if(duckTapeCount > 0 && axeBodyCollected && axeHeadCollected)
        {
            //craftableAxe.color = showColour;
            craftableAxe.interactable = true;
        }
        else
        {
            //craftableAxe.color = hideColour;
            craftableAxe.interactable = false;
        }
    }

    void CheckLadderCraftable()
    {
        if (duckTapeCount > 0 && ladder1Collected && ladder2Collected)
        {
            //craftableLadder.color = showColour;
            craftableLadder.interactable = true;
            Debug.Log("Can craft ladder");
        }
        else
        {
            //craftableLadder.color = hideColour;
            craftableLadder.interactable = false;
        }
    }

    void UpdateDuckTapeUI()
    {
        if(duckTapeCount > 0)
        {
            if(!axeCrafted)
                axeDuckTape.color = showColour;

            if(!ladderCrafted)
                ladderDuckTape.color = showColour;
        }
        else
        {
            axeDuckTape.color = hideColour;
            ladderDuckTape.color = hideColour;
        }

        CheckAxeCraftable();
        CheckLadderCraftable();
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
