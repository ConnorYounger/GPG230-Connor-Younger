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
}
