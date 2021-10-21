using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4LevelTimer : MonoBehaviour
{
    public float levelTimer;
    void Update()
    {
        levelTimer += Time.deltaTime;
    }
}
