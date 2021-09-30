using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2Item : MonoBehaviour
{
    [System.Serializable] public enum itemEnum { key, axe, ladder, map, motionSensor, santiyPills };
    public itemEnum itemType;

    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        GameObject.Find("MainCamera").GetComponent<MouseInteraction>().AddItem(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
