using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2Interractable : MonoBehaviour
{
    [System.Serializable] public enum itemEnum { key, axe, ladder, map, motionSensor, santiyPills, frontDoor, window, highWindow, workBench, 
                                                duckTape, axeBody, axeHead, ladderBottom, ladderTop, barricadedWindow, safe, safeCode1, safeCode2 };
    public itemEnum interractableType;

    public bool isItem = true;
    public float interactDistance = 0.8f;

    public string itemName;
    [TextArea] public string[] dialogueTexts;

    public Sprite itemSprite;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<Outline>())
            outline = gameObject.GetComponent<Outline>();
        GameObject.Find("MainCamera").GetComponent<MouseInteraction>().AddItem(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseOver(Color color)
    {
        if (outline)
        {
            outline.OutlineColor = color;
            outline.enabled = true;

            StopCoroutine("DelayedHideOutline");
            StartCoroutine("DelayedHideOutline");
        }
    }

    public void ShowOutline(Color color)
    {
        if (gameObject.active && outline)
        {
            outline.OutlineColor = color;
            outline.enabled = true;

            StopCoroutine("DelayedHideOutline2");
            StartCoroutine("DelayedHideOutline2");
        }
    }

    public void CreateItem(string type)
    {
        switch (type)
        {
            case "axe":
                interractableType = itemEnum.axe;
                break;
            case "ladder":
                interractableType = itemEnum.ladder;
                break;
        }
    }

    public void HideOutline()
    {
        if (outline)
            outline.enabled = false;
    }

    IEnumerator DelayedHideOutline()
    {
        yield return new WaitForSeconds(0.1f);

        if (outline)
            outline.enabled = false;
    }

    IEnumerator DelayedHideOutline2()
    {
        yield return new WaitForSeconds(1.5f);

        if (outline)
            outline.enabled = false;
    }

    private void OnDisable()
    {
        if(outline)
            outline.enabled = false;
    }
}
