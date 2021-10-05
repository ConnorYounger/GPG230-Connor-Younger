using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2Door : MonoBehaviour
{
    private RoomManager roomManager;
    private Outline outline;

    public int attachedRoomIndex;
    public int entranceIndex;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();

        if(gameObject.GetComponent<Outline>())
            outline = gameObject.GetComponent<Outline>();

        GameObject.Find("MainCamera").GetComponent<MouseInteraction>().AddDoor(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseDoor()
    {
        roomManager.TravelToNewRoom(attachedRoomIndex, entranceIndex);
    }

    public void MouseOver(Color color)
    {
        if (outline)
        {
            outline.OutlineColor = color;
            outline.enabled = true;
        }

        StopCoroutine("DelayedHideOutline");
        StartCoroutine("DelayedHideOutline");
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

    public void HideOutline()
    {
        if(outline)
            outline.enabled = false;
    }

    IEnumerator DelayedHideOutline()
    {
        yield return new WaitForSeconds(0.1f);

        if(outline)
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
