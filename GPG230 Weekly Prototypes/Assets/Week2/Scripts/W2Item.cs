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
