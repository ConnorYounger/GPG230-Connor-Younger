using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2Inventory : MonoBehaviour
{
    public W2Item key;
    public W2Item axe;
    public W2Item ladder;
    public W2Item map;
    public W2Item motionSensor;
    public W2Item sanityPills;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(W2Item item)
    {
        switch (item.itemType.ToString())
        {
            case "key":
                if(key != null)
                    key = item;
                break;
            case "axe":
                if (axe != null)
                    axe = item;
                break;
            case "ladder":
                if (ladder != null)
                    ladder = item;
                break;
            case "map":
                if (map != null)
                    map = item;
                break;
            case "motionSensor":
                if (motionSensor != null)
                    motionSensor = item;
                break;
            case "sanityPills":
                if (sanityPills != null)
                    sanityPills = item;
                break;
        }
    }
}
