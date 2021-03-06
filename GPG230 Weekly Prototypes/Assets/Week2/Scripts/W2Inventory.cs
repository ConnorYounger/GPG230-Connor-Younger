using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class W2Inventory : MonoBehaviour
{
    public W2Player player;

    public List<W2Interractable> items;

    public W2Safe safeManager;

    public Image keyImg;
    public Image axeImg;
    public Image ladderImg;
    public Image mapImg;
    public Image motionSensorImg;
    public Image sanityPillsImg;

    [Header("Workbench UI")]
    public Image axeBodyImage;
    public Image axeHeadImage;
    public Image axeDuckTape;
    public Image ladderDuckTape;
    public Image ladderHalf1;
    public Image ladderHalf2;
    public Button craftableAxe;
    public Button craftableLadder;

    [Header("Inventory UI")]
    public GameObject baseInventorySlot;
    public Transform inventoryGroup;

    public GameObject pickUpItemUI;
    public Image itemDisplaySlot;
    public TMP_Text itemNameField;
    public TMP_Text itemTextField;
    public TMP_Text itemSafeCodeField;
    public TMP_Text itemSafeCode2Field;
    public GameObject mapButton;
    public W2AchievementsManager achivementManager;

    public GameObject axeItem;
    public GameObject ladderItem;

    public Color showColour;
    public Color hideColour;

    private int duckTapeCount;

    private bool axeBodyCollected;
    private bool axeHeadCollected;
    private bool ladder1Collected;
    private bool ladder2Collected;
    private bool axeCrafted;
    private bool ladderCrafted;

    [Header("Audio")]
    public AudioSource audioSource;

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
                    mapButton.SetActive(true);
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
                case "excalibur":
                    achivementManager.UnlockAchievement("excalibur");
                    break;
                case "imSoCool":
                    achivementManager.UnlockAchievement("I'm So Cool");
                    break;
            }

            AddItem(item);
        }

        Debug.Log("Added: " + item);
    }

    public void CraftAxe()
    {
        // Item creation
        GameObject axe = Instantiate(axeItem, transform.position, transform.rotation);
        PickUpItem(axe.GetComponent<W2Interractable>());
        axeCrafted = true;
        craftableAxe.interactable = false;

        // Material costs
        axeBodyCollected = false;
        axeHeadCollected = false;
        axeHeadImage.color = hideColour;
        axeBodyImage.color = hideColour;
        duckTapeCount--;
        RemoveItem(SearchForItem("axeBody"));
        RemoveItem(SearchForItem("axeHead"));
        RemoveItem(SearchForItem("duckTape"));
        UpdateDuckTapeUI();

        player.HideWorkBenchUI();
    }

    public void CraftLadder()
    {
        // Item creation
        GameObject newItem = Instantiate(ladderItem, transform.position, transform.rotation);
        PickUpItem(newItem.GetComponent<W2Interractable>());
        ladderCrafted = true;
        craftableLadder.interactable = false;

        // Material costs
        ladder1Collected = false;
        ladder2Collected = false;
        ladderHalf1.color = hideColour;
        ladderHalf2.color = hideColour;
        duckTapeCount--;
        RemoveItem(SearchForItem("ladderBottom"));
        RemoveItem(SearchForItem("ladderTop"));
        RemoveItem(SearchForItem("duckTape"));
        UpdateDuckTapeUI();

        player.HideWorkBenchUI();
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

        // Inventory UI
        GameObject newSlot = Instantiate(baseInventorySlot, inventoryGroup.position, inventoryGroup.rotation);
        newSlot.transform.parent = inventoryGroup.transform;
        newSlot.GetComponent<W2ItemSlot>().itemType = item.interractableType.ToString();

        if (item.interractableType.ToString() == "safeCode1")
        {
            newSlot.transform.GetChild(1).GetComponent<TMP_Text>().text = safeManager.safeCodeFirstHalf;
        }
        else if (item.interractableType.ToString() == "safeCode2")
        {
            newSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = safeManager.safeCodeSecondHalf;
        }

        if (item.itemSprite)
            newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;

        if (item.puckUpSound)
        {
            audioSource.clip = item.puckUpSound;
            audioSource.Play();
        }

        CheckForAchieve();

        StopCoroutine("DisplayPickUpItemUI");
        StartCoroutine("DisplayPickUpItemUI", item);
    }

    void RemoveItem(W2Interractable item)
    {
        GameObject itemToRemove = null;

        foreach(Transform child in inventoryGroup.transform)
        {
            if(child.GetComponent<W2ItemSlot>().itemType == item.interractableType.ToString())
            {
                itemToRemove = child.gameObject;
            }
        }

        items.Remove(item);

        if (itemToRemove != null)
            Destroy(itemToRemove);
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

    public IEnumerator DisplayPickUpItemUI(W2Interractable item)
    {
        itemNameField.text = "Collected " + item.itemName + "!";

        if(item.itemSprite != null)
            itemDisplaySlot.sprite = item.itemSprite;

        if (item.dialogueTexts.Length > 0)
            itemTextField.text = item.dialogueTexts[0];

        if(item.interractableType.ToString() == "safeCode1")
        {
            itemSafeCodeField.text = safeManager.safeCodeFirstHalf;
            itemSafeCode2Field.text = "";
        }
        else if (item.interractableType.ToString() == "safeCode2")
        {
            itemSafeCode2Field.text = safeManager.safeCodeSecondHalf;
            itemSafeCodeField.text = "";
        }
        else
        {
            itemSafeCodeField.text = "";
            itemSafeCode2Field.text = "";
        }

        pickUpItemUI.SetActive(true);

        yield return new WaitForSeconds(7);

        CloseItemPickUpDisplay();
    }

    public void CloseItemPickUpDisplay()
    {
        pickUpItemUI.SetActive(false);
    }

    void CheckForAchieve()
    {
        bool key = false;
        bool axe = false;
        bool ladder = false;

        foreach(W2Interractable item in items)
        {
            if (item.interractableType.ToString() == "key" && !key)
                key = true;
            else if (item.interractableType.ToString() == "axe" && !axe)
            {
                axe = true;
            }
            else if (item.interractableType.ToString() == "ladder" && !ladder)
            {
                ladder = true;
            }
        }

        if(key && axe && ladder)
        {
            achivementManager.UnlockAchievement("allWin");
        }
    }
}
