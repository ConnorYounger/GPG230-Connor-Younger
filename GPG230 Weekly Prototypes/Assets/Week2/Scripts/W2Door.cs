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
        outline.OutlineColor = color;
        outline.enabled = true;

        StopCoroutine("DelayedHideOutline");
        StartCoroutine("DelayedHideOutline");
    }

    public void ShowOutline(Color color)
    {
        outline.OutlineColor = color;
        outline.enabled = true;

        StopCoroutine("DelayedHideOutline2");
        StartCoroutine("DelayedHideOutline2");
    }

    public void HideOutline()
    {
        outline.enabled = false;
    }

    IEnumerator DelayedHideOutline()
    {
        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;
    }

    IEnumerator DelayedHideOutline2()
    {
        yield return new WaitForSeconds(1.5f);

        outline.enabled = false;
    }
}
